using Microsoft.EntityFrameworkCore;
using RB.Domain.Entities;

namespace RB.Application.Interfaces
{
    public interface IRBDbContext
    {
        DbSet<Booking> Bookings { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TeamMember> TeamMembers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
