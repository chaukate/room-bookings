using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : BaseConfiguration<Client>
    {
        public override void Configure(EntityTypeBuilder<Client> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.AdminEmail)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.SecretKey)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(p => p.TenantId)
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
