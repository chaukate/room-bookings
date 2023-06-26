using RB.Domain.Enumerations;

namespace RB.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public BookingStatus Status { get; set; }
        public int BookedBy { get; set; }
        public int BookedFor { get; set; }
        public DateTimeOffset BookedAt { get; set; }
        public string Description { get; set; }

        public virtual Member Member { get; set; } = new Member();
        public virtual Team Team { get; set; } = new Team();
    }
}
