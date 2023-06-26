namespace RB.Domain.Entities
{
    public class Member : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool HasAccess { get; set; }

        public virtual ICollection<TeamMember> MemberTeams { get; set; }
    }
}
