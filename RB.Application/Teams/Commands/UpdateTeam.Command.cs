using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using System.Text.Json.Serialization;

namespace RB.Application.Teams.Commands
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly IRBDbContext _dbContext;
        private readonly ISlackService _slackService;
        public UpdateTeamHandler(IRBDbContext dbContext,
                                 ISlackService slackService)
        {
            _dbContext = dbContext;
            _slackService = slackService;
        }

        public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var dbTeam = await _dbContext.Teams.FindAsync(request.Id);
            if (dbTeam == null)
            {
                throw new NotFoundException();
            }

            if (await _dbContext.Teams.AnyAsync(a => a.Id != request.Id &&
                                                     a.Name.ToLower() == request.Name.ToLower(),
                                                     cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            if (dbTeam.LeadId != request.LeadId)
            {
                var dbMember = await _dbContext.Members.FindAsync(request.LeadId);
                if (dbMember == null)
                {
                    throw new BadRequestException("Invalid lead request.");
                }

                // TODO : Need to implement this in event
                var slackUser = await _slackService.GetUserByEmailAsync(dbMember.Email, cancellationToken);
                if (slackUser.Ok && slackUser.User != null)
                {
                    dbMember.SlackUserId = slackUser.User.Id;
                    dbMember.SlackUserImage = slackUser.User.Profile.Image;
                    dbMember.HasAccess = true;
                    dbMember.LastUpdatedAt = DateTimeOffset.UtcNow;
                    dbMember.LastUpdatedBy = request.CurrentUser;

                    await _slackService.InviteMemberToChannelAsync(slackUser.User.Id, cancellationToken);
                }

                if (_dbContext.Teams.Count(c => c.LeadId == dbTeam.LeadId) <= 1)
                {
                    var dbOldMember = await _dbContext.Members.FindAsync(dbTeam.LeadId);
                    await _slackService.RemoveMemberFromChannelAsync(dbOldMember.SlackUserId, cancellationToken);

                    dbOldMember.HasAccess = false;
                    dbOldMember.LastUpdatedAt = DateTimeOffset.UtcNow;
                    dbOldMember.LastUpdatedBy = request.CurrentUser;
                }
            }

            dbTeam.Name = request.Name;
            dbTeam.Description = request.Description;
            dbTeam.LeadId = request.LeadId;
            dbTeam.IsActive = true;
            dbTeam.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbTeam.LastUpdatedBy = request.CurrentUser;

            _dbContext.Teams.Update(dbTeam);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateTeamCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
