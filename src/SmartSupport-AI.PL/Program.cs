using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.BLL.Services.Classes.Tickets;
using SmartSupport_AI.BLL.Services.Interfaces.Tickets;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Repositories.Classes.Tickets;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;
using SmartSupport_AI.BLL.Services.Classes.Orders;
using SmartSupport_AI.BLL.Services.Interfaces.Orders;
using SmartSupport_AI.DAL.Repositories.Classes.Orders;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;
using SmartSupport_AI.BLL.Services.Classes.Conversations;
using SmartSupport_AI.BLL.Services.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Classes.Conversations;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Classes.KnowledgeBase;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;
using SmartSupport_AI.PL.Services.FileStorage;

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

            builder.Services.AddScoped<IAIConfigurationRepository, AIConfigurationRepository>();

            // Conversations & Messaging module
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

            // Tickets module
            builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
            builder.Services.AddScoped<ITicketAssignmentRepository, TicketAssignmentRepository>();
            builder.Services.AddScoped<IInternalNoteRepository, InternalNoteRepository>();
            builder.Services.AddScoped<ITicketStatusHistoryRepository, TicketStatusHistoryRepository>();

            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IInternalNoteService, InternalNoteService>();

            // Orders module
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderStatusCheckRepository, OrderStatusCheckRepository>();
            builder.Services.AddScoped<IRefundRequestRepository, RefundRequestRepository>();
            builder.Services.AddScoped<IRefundStatusHistoryRepository, RefundStatusHistoryRepository>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IRefundService, RefundService>();

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
