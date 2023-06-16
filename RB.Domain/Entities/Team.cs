﻿namespace RB.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int LeadId { get; set; }

        public virtual Member Lead { get; set; } = new Member();
    }
}