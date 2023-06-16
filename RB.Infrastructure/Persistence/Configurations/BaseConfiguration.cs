using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1);

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.Property(p => p.LastUpdatedAt)
                .IsRequired();

            builder.Property(p => p.LastUpdatedBy)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
