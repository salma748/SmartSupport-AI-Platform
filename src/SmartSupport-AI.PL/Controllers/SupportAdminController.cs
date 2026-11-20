using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SmartSupport_AI.BLL;

namespace SmartSupport_AI.PL.Controllers;

[Authorize(Roles = "Admin")]
[EnableRateLimiting("support")]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public sealed class SupportAdminController(SupportAdministration admin, IAIProvider provider, IWebHostEnvironment environment) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct) => View(await admin.ReadAsync(ct));

    [HttpPost]
    public Task<IActionResult> Article(int id, string? title, string? category, string? content, bool publish, CancellationToken ct) =>
        UpdateAsync(actor => admin.ArticleAsync(actor, id, title ?? "", category ?? "", content ?? "", publish, ct), ct);

    [HttpPost]
    public Task<IActionResult> Policy(string? name, string? value, CancellationToken ct) =>
        UpdateAsync(actor => admin.PolicyAsync(actor, name ?? "", value?.Trim() ?? "", ct), ct);

    [HttpPost]
    public async Task<IActionResult> Probe(CancellationToken ct)
    {
        if (!environment.IsDevelopment()) return NotFound();
        var result = await provider.GenerateAsync("Reply with OK.",
            [new { role = "user", parts = new[] { new { text = "Synthetic connectivity check" } } }], null, ct);
        TempData["SupportAdminStatus"] = result is null ? "Gemini connectivity could not be verified. Check provider configuration and quota." : "Gemini returned a response.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> UpdateAsync(Func<DeskIdentity, Task> update, CancellationToken ct)
    {
        if (await SupportPrincipal.ResolveAsync(HttpContext) is not { Role: "Admin" } actor) return Forbid();
        try
        {
            await update(actor);
            TempData["SupportAdminStatus"] = "Saved.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(nameof(Index), await admin.ReadAsync(ct));
        }
    }
}
