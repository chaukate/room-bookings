using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Members.Commands
{
    public class AssignMemberToTeamsHandler : IRequestHandler<AssignMemberToTeamsCommand>
    {
        private readonly IRBDbContext _dbContext;
        public AssignMemberToTeamsHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AssignMemberToTeamsCommand request, CancellationToken cancellationToken)
        {
            var dbMember = await _dbContext.Members.Include(i => i.MemberTeams)
                                                   .FirstOrDefaultAsync(fd => fd.Id == request.MemberId, cancellationToken);
            if (dbMember == null)
            {
                throw new NotFoundException();
            }

            var teamsToRemove = dbMember.MemberTeams.Where(w => !request.TeamsId.Contains(w.TeamId));
            foreach (var teamToRemove in teamsToRemove)
            {
                dbMember.MemberTeams.Remove(teamToRemove);
            }

            foreach (var teamId in request.TeamsId)
            {
                var dbMemberTeam = dbMember.MemberTeams.FirstOrDefault(fd => fd.TeamId == teamId);
                if (dbMemberTeam == null)
                {
                    dbMemberTeam = new TeamMember
                    {
                        MemberId = dbMember.Id,
                        TeamId = teamId,
                        IsActive = true,
                        LastUpdatedBy = request.CurrentUser
                    };
                    dbMember.MemberTeams.Add(dbMemberTeam);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class AssignMemberToTeamsCommand : IRequest
    {
        public int MemberId { get; set; }
        public int[] TeamsId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
