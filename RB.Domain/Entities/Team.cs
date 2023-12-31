﻿using RB.Domain.Interfaces;

namespace RB.Domain.Entities
{
    public class Team : BaseEntity, ICreatedEvent, IUpdatedEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }

        public virtual Member Lead { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
