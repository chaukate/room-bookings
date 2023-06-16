using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public class BookingConfiguration : BaseConfiguration<Booking>
    {
        public override void Configure(EntityTypeBuilder<Booking> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.StartAt)
                .IsRequired();

            builder.Property(p => p.EndAt)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.HasOne(p => p.Member)
                .WithMany()
                .HasForeignKey(h => h.BookedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(h => h.BookedFor)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();
        }
    }
}
