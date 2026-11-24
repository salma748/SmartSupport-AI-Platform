using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.BLL.Services.Classes.Tickets;
using SmartSupport_AI.BLL.Services.Interfaces.Tickets;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Repositories.Classes.Tickets;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;


namespace SmartSupport_AI.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            builder.Services.AddDbContext<SmartSupportDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Tickets module
            builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
            builder.Services.AddScoped<ITicketAssignmentRepository, TicketAssignmentRepository>();
            builder.Services.AddScoped<IInternalNoteRepository, InternalNoteRepository>();
            builder.Services.AddScoped<ITicketStatusHistoryRepository, TicketStatusHistoryRepository>();

            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IInternalNoteService, InternalNoteService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
