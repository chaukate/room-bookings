using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Events;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using RB.Domain.Interfaces;
using System.Reflection;

namespace RB.Infrastructure.Persistence
{
    public class RBDbContext : DbContext, IRBDbContext
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        public RBDbContext(DbContextOptions<RBDbContext> options,
                           IEventDispatcherService eventDispatcherService) : base(options)
        {
            _eventDispatcherService = eventDispatcherService;
        }

        // Arrange all dbsets alphabetically
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Client> Clients { get; set; }
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
            QueueDomainEvents();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void QueueDomainEvents()
        {
            var createdEntities = ChangeTracker.Entries<ICreatedEvent>().Where(w => w.State == EntityState.Added).ToList();
            foreach (var createdEntity in createdEntities)
            {
                var entity = new CreatedEvent(createdEntity.Entity);
                _eventDispatcherService.QueueNotification(entity);
            }

            var updatedEntities = ChangeTracker.Entries<IUpdatedEvent>().Where(w => w.State == EntityState.Modified).ToList();
            foreach (var updatedEntity in updatedEntities)
            {
                var entity = new UpdatedEvent(updatedEntity.Entity);
                _eventDispatcherService.QueueNotification(entity);
            }
        }
    }
}
