using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.BLL;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories;
using SmartSupport_AI.PL;

var builder = WebApplication.CreateBuilder(args);
using var localGeminiSecrets = builder.Environment.IsDevelopment()
    ? new ConfigurationBuilder().SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("gemini.secrets.json", optional: true).Build() as ConfigurationRoot
    : null;
var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? (args.Contains("--verify-gemini") ? "" : throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required."));
builder.Services.AddDbContext<SmartSupportDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<SupportUnitOfWork>();
builder.Services.AddScoped<SupportDesk>();
builder.Services.AddScoped<SupportAdministration>();
builder.Services.AddSingleton<AgentPresence>();
builder.Services.AddSingleton<ISupportPresence>(services => services.GetRequiredService<AgentPresence>());
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddHttpClient("Gemini")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false })
    .RedactLoggedHeaders(_ => true);
builder.Services.AddScoped<IAIProvider>(services => new GeminiAIProvider(
    services.GetRequiredService<IHttpClientFactory>().CreateClient("Gemini"),
    builder.Configuration.GetSection("Gemini").Get<AIProviderOptions>() ?? new(),
    builder.Configuration["GEMINI_API_KEY"] ?? localGeminiSecrets?["GEMINI_API_KEY"]));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Support/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(4);
    options.Events.OnValidatePrincipal = async context =>
    {
        var identity = await SupportPrincipal.ResolveAsync(context.HttpContext, context.Principal);
        if (identity is null || !context.Principal!.IsInRole(identity.Role))
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync();
        }
    };
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("support", context => RateLimitPartition.GetFixedWindowLimiter(
        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 60, Window = TimeSpan.FromMinutes(1) }));
    options.AddPolicy("login", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) }));
});
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
var app = builder.Build();
if (args.Contains("--verify-gemini"))
{
    using var scope = app.Services.CreateScope();
    return await GeminiConnectivityCheck.RunAsync(
        scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Gemini"),
        builder.Configuration.GetSection("Gemini").Get<AIProviderOptions>() ?? new(),
        builder.Configuration["GEMINI_API_KEY"] ?? localGeminiSecrets?["GEMINI_API_KEY"]);
}
if (args.Contains("--migrate-support-test"))
{
    if (!app.Environment.IsDevelopment()) throw new InvalidOperationException("Test migration requires Development.");
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<SmartSupportDbContext>();
    if (!string.Equals(db.Database.GetDbConnection().Database, "SmartSupportDB_Test", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("Test migration requires SmartSupportDB_Test.");
    await db.Database.MigrateAsync();
    return 0;
}
if (args.Contains("--provision-support"))
{
    if (!app.Environment.IsDevelopment()) throw new InvalidOperationException("Provisioning is restricted to Development.");
    await using var scope = app.Services.CreateAsyncScope();
    await SupportProvisioning.RunAsync(scope.ServiceProvider, CancellationToken.None);
    return 0;
}
app.UseExceptionHandler(error => error.Run(async context =>
{
    context.Response.StatusCode = 500;
    await context.Response.WriteAsync("Support is temporarily unavailable. Reopen the conversation to verify its current state.");
}));
app.UseRouting();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapStaticAssets();
app.MapHub<SupportHub>("/support-live", options => options.CloseOnAuthenticationExpiration = true);
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
await app.RunAsync();
return 0;

public partial class Program;
