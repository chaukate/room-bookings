using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text;
using System.Text.Json.Serialization;

namespace RB.Application.Teams.Commands
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, int>
    {
        private readonly IRBDbContext _dbContext;
        private readonly ISlackService _slackService;
        private readonly IGraphEmailService _graphEmailService;
        public CreateTeamHandler(IRBDbContext dbContext,
                                 ISlackService slackService,
                                 IGraphEmailService graphEmailService)
        {
            _dbContext = dbContext;
            _slackService = slackService;
            _graphEmailService = graphEmailService;
        }

        public async Task<int> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var dbMember = await _dbContext.Members.FindAsync(request.LeadId);
            if (dbMember == null)
            {
                throw new BadRequestException("Invalid lead request.");
            }

            if (await _dbContext.Teams.AnyAsync(a => a.Name.ToLower() == request.Name.ToLower(), cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                LeadId = request.LeadId,
                IsActive = true,
                LastUpdatedBy = request.CurrentUser
            };

            // TODO : Need to implement this in event
            if (string.IsNullOrEmpty(dbMember.SlackUserId) || !dbMember.HasAccess)
            {
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
            }

            //var content = new StringBuilder("You have been assigned to team ");
            //content.AppendLine($"<b>{request.Name}</b>");
            //await _graphEmailService.SendMailAsync(dbMember.Email, "Assigned To Team", content, cancellationToken);

            _dbContext.Teams.Add(team);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }

    public class CreateTeamCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
