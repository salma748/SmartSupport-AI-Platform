using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using SmartSupport_AI.BLL;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.PL.Controllers;

[Authorize]
[EnableRateLimiting("support")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public sealed class SupportController(SupportDesk desk, IPasswordHasher<User> passwords, IHubContext<SupportHub> live) : Controller
{
    [AllowAnonymous, HttpGet]
    public IActionResult Login() => View();

    [AllowAnonymous, HttpPost, EnableRateLimiting("login")]
    public async Task<IActionResult> Login(string? email, string? password, CancellationToken ct)
    {
        if (email is { Length: > 0 and <= 256 } && password is { Length: > 0 and <= 256 })
        {
            var user = await desk.FindUserAsync(email.Trim(), ct);
            if (user is not null && passwords.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed &&
                await desk.IdentityAsync(user.UserId, ct) is { } identity)
            {
                var principal = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.NameIdentifier, identity.UserId.ToString()), new Claim(ClaimTypes.Name, identity.Name),
                     new Claim(ClaimTypes.Role, identity.Role)], CookieAuthenticationDefaults.AuthenticationScheme));
                await HttpContext.SignInAsync(principal);
                return identity.Role == "Admin" ? RedirectToAction("Index", "SupportAdmin") : RedirectToAction(nameof(Index));
            }
        }
        ModelState.AddModelError("", "The email or password was not accepted.");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet, Authorize(Roles = "Customer,Agent")]
    public IActionResult Index() => View();

    [HttpGet, Authorize(Roles = "Customer,Agent")]
    public Task<IActionResult> Dashboard(CancellationToken ct) => RunAsync(async actor =>
        Json(new { tickets = await desk.TicketsAsync(actor, ct), conversations = await desk.ConversationsAsync(actor, ct) }));

    [HttpPost, Authorize(Roles = "Customer")]
    public Task<IActionResult> Start(CancellationToken ct) => RunAsync(async actor => Json(new { id = await desk.StartAsync(actor, ct) }));

    [HttpGet, Authorize(Roles = "Customer,Agent")]
    public Task<IActionResult> Thread(int id, CancellationToken ct) => RunAsync(async actor => Json(await desk.ReadAsync(actor, id, ct)));

    [HttpPost, Authorize(Roles = "Customer,Agent")]
    public Task<IActionResult> Send(int id, string? message, Guid? requestId, CancellationToken ct) => RunAsync(async actor =>
    {
        var result = await desk.SendAsync(actor, id, message ?? "", ct, requestId);
        await NotifyAsync(id, ct);
        return Json(result);
    });

    [HttpPost, Authorize(Roles = "Customer")]
    public Task<IActionResult> Refund(int id, int orderId, string? reason, CancellationToken ct) => RunAsync(async actor =>
    {
        var refundId = await desk.ConfirmRefundAsync(actor, id, orderId, reason ?? "", ct);
        await NotifyAsync(id, ct);
        return Json(new { refundId });
    });

    [HttpPost, Authorize(Roles = "Agent")]
    public Task<IActionResult> Ticket(int ticketId, string action, string? note, CancellationToken ct) => RunAsync(async actor =>
    {
        var id = await desk.AgentActionAsync(actor, ticketId, action, note, ct);
        await NotifyAsync(id, ct);
        return Json(new { id });
    });

    [HttpPost, Authorize(Roles = "Customer")]
    public Task<IActionResult> Feedback(int id, int rating, CancellationToken ct) => RunAsync(async actor =>
        Json(new { rating = await desk.FeedbackAsync(actor, id, rating, ct) }));

    private async Task NotifyAsync(int id, CancellationToken ct)
    {
        // Only send invalidations. Every fetch rechecks the current user's authorization.
        await live.Clients.Groups("agents", $"user:{await desk.CustomerUserIdAsync(id, ct)}").SendAsync("Changed", cancellationToken: ct);
    }

    private async Task<IActionResult> RunAsync(Func<DeskIdentity, Task<IActionResult>> operation)
    {
        try
        {
            var actor = await SupportPrincipal.ResolveAsync(HttpContext);
            return actor is null || !User.IsInRole(actor.Role) ? Forbid() : await operation(actor);
        }
        catch (UnauthorizedAccessException) { return StatusCode(403, new { error = "This operation is unavailable for your account." }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException) { return Conflict(new { error = "The operation could not be completed. Refresh to check the current state." }); }
    }
}
