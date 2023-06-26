using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Teams.Commands
{
    public class AllocateTeamMembersHandler : IRequestHandler<AllocateTeamMembersCommand>
    {
        private readonly IRBDbContext _dbContext;
        public AllocateTeamMembersHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AllocateTeamMembersCommand request, CancellationToken cancellationToken)
        {
            var dbTeam = await _dbContext.Teams.Include(i => i.TeamMembers)
                                               .FirstOrDefaultAsync(fd => fd.Id == request.TeamId, cancellationToken);
            if (dbTeam == null)
            {
                throw new NotFoundException();
            }

            var membersToRemove = dbTeam.TeamMembers.Where(w => !request.MembersId.Contains(w.MemberId));
            foreach (var memberToRemove in membersToRemove)
            {
                dbTeam.TeamMembers.Remove(memberToRemove);
            }

            foreach (var memberId in request.MembersId)
            {
                var dbTeamMember = dbTeam.TeamMembers.FirstOrDefault(fd => fd.MemberId == memberId);
                if (dbTeamMember == null)
                {
                    dbTeamMember = new TeamMember
                    {
                        MemberId = memberId,
                        TeamId = request.TeamId,
                        IsActive = true,
                        LastUpdatedBy = request.CurrentUser
                    };
                    dbTeam.TeamMembers.Add(dbTeamMember);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class AllocateTeamMembersCommand : IRequest
    {
        [JsonIgnore]
        public int TeamId { get; set; }

        public int[] MembersId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
