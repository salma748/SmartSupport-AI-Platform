using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.CustomerId);

            builder.Property(e => e.PreferredLanguage).IsRequired();

            builder.Property(e => e.RegisteredAt).IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Customer>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Conversation>()
                .WithOne()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<CustomerFeedback>()
                .WithOne()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Message>()
                .WithOne()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<SupportTicket>()
                .WithOne()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Order>()
                .WithOne()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
