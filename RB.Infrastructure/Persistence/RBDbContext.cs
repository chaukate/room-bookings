using Microsoft.EntityFrameworkCore;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Reflection;

namespace RB.Infrastructure.Persistence
{
    public class RBDbContext : DbContext, IRBDbContext
    {
        public RBDbContext(DbContextOptions<RBDbContext> options) : base(options) { }

        // Arrange all dbsets alphabetically
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
