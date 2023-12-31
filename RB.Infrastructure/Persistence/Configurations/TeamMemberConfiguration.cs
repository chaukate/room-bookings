﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RB.Domain.Entities;

namespace RB.Infrastructure.Persistence.Configurations
{
    public class TeamMemberConfiguration : BaseConfiguration<TeamMember>
    {
        public override void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            base.Configure(builder);

            builder.HasOne(h => h.Team)
                .WithMany(w => w.TeamMembers)
                .HasForeignKey(h => h.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Member)
                .WithMany(w => w.MemberTeams)
                .HasForeignKey(h => h.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
