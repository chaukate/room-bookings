using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(h => h.Email)
                .IsUnique();

            builder.Property(p => p.SlackUserId)
                .HasMaxLength(100)
                .IsRequired(false);
            builder.HasIndex(h => h.SlackUserId)
                .IsUnique();

            builder.Property(p => p.SlackUserImage)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(p => p.IsActive);
        }
    }
}
