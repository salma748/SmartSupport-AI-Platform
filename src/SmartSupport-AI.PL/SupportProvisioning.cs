using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.BLL;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories;

namespace SmartSupport_AI.PL;

public static class SupportProvisioning
{
    public static async Task RunAsync(IServiceProvider services, CancellationToken ct)
    {
        var db = services.GetRequiredService<SmartSupportDbContext>();
        if (!string.Equals(db.Database.GetDbConnection().Database, "SmartSupportDB_Test", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Test provisioning requires SmartSupportDB_Test.");
        var hasher = services.GetRequiredService<IPasswordHasher<User>>();
        var passwords = new[] { "CUSTOMER", "AGENT", "ADMIN" }.ToDictionary(role => role,
            role => Environment.GetEnvironmentVariable($"SUPPORT_TEST_{role}_PASSWORD"));
        if (passwords.Values.Any(password => password is not { Length: >= 12 and <= 256 }))
            throw new InvalidOperationException("Set all three SUPPORT_TEST role passwords to at least 12 characters.");
        var suffix = Environment.GetEnvironmentVariable("SUPPORT_TEST_ACCOUNT_SUFFIX") ?? "";
        if (suffix.Length > 32 || suffix.Any(c => !char.IsAsciiLetterOrDigit(c)))
            throw new InvalidOperationException("The optional test account suffix must be alphanumeric.");
        var accountSuffix = suffix.Length == 0 ? "" : "-" + suffix;
        var work = services.GetRequiredService<SupportUnitOfWork>();
        await work.AtomicAsync("support-provisioning", async () =>
        {
            foreach (var item in passwords)
            {
                var email = $"{item.Key.ToLowerInvariant()}{accountSuffix}@smartsupport.test";
                if (await db.Users.AnyAsync(u => u.Email == email, ct)) continue;
                var user = new User { FullName = $"Test {item.Key}", Email = email, Status = "Active", CreatedAt = DateTime.UtcNow };
                user.PasswordHash = hasher.HashPassword(user, item.Value!);
                switch (item.Key)
                {
                    case "CUSTOMER":
                        user.Role = "Customer";
                        user.Customer = new Customer { PreferredLanguage = "en", RegisteredAt = DateTime.UtcNow };
                        db.Orders.Add(new Order { Customer = user.Customer, OrderNumber = "TEST-1001" + accountSuffix, OrderStatus = "Delivered",
                            TotalAmount = 25m, PlacedAt = DateTime.UtcNow });
                        break;
                    case "AGENT":
                        user.Role = "Agent";
                        user.SupportAgent = new SupportAgent { Department = "Support", SkillLevel = "Standard", Availability = "Enabled" };
                        break;
                    default:
                        user.Role = "Admin";
                        user.Administrator = new Administrator { AccessLevel = "Full" };
                        break;
                }
                db.Users.Add(user);
            }
            await db.SaveChangesAsync(ct);
            var admin = await db.Administrators.SingleAsync(a => a.User.Email == "admin" + accountSuffix + "@smartsupport.test", ct);
            if (!await db.AIConfigurations.AnyAsync(c => c.IsActive, ct))
                db.AIConfigurations.Add(new AIConfiguration { CreatedBy = admin.AdminId, IsActive = true,
                    ModelName = "gemini-3.7-flash", VersionNumber = SmartSupportPrompt.Version, CreatedAt = DateTime.UtcNow });
            foreach (var intent in Enum.GetNames<SupportIntent>())
                if (!await db.Intents.AnyAsync(i => i.Name == intent, ct)) db.Intents.Add(new Intent { Name = intent });
            return true;
        }, ct);
        Console.WriteLine("Test support accounts and configuration are ready.");
    }
}
