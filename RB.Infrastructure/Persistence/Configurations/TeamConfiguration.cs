using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public class TeamConfiguration : BaseConfiguration<Team>
    {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();

            builder.HasOne(p => p.Lead)
                .WithMany()
                .HasForeignKey(h => h.LeadId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
