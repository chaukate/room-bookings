namespace RB.Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        public int TeamId { get; set; }
        public int MemberId { get; set; }

        public virtual Team Team { get; set; } = new Team();
        public virtual Member Member { get; set; } = new Member();
    }
}
