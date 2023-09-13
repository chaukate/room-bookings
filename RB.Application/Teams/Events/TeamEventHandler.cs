using MediatR;
using RB.Application.Common.Events;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text;

namespace RB.Application.Teams.Events
{
    public class TeamEventHandler : INotificationHandler<CreatedEvent>, INotificationHandler<UpdatedEvent>
    {
        private readonly IRBDbContext _dbContext;
        private readonly ISlackService _slackService;
        private readonly IGraphEmailService _graphEmailService;
        public TeamEventHandler(IRBDbContext dbContext,
                                 ISlackService slackService,
                                 IGraphEmailService graphEmailService)
        {
            _dbContext = dbContext;
            _slackService = slackService;
            _graphEmailService = graphEmailService;
        }

        public async Task Handle(CreatedEvent notification, CancellationToken cancellationToken)
        {
            var team = notification.GetEntity<Team>();
            if (team != null)
            {
                var lead = await _dbContext.Members.FindAsync(team.LeadId);

                if (string.IsNullOrEmpty(lead.SlackUserId) || !lead.HasAccess)
                {
                    var slackUser = await _slackService.GetUserByEmailAsync(lead.Email, cancellationToken);
                    if (slackUser.Ok && slackUser.User != null)
                    {
                        lead.SlackUserId = slackUser.User.Id;
                        lead.SlackUserImage = slackUser.User.Profile.Image;
                        lead.HasAccess = true;
                        lead.LastUpdatedAt = DateTimeOffset.UtcNow;
                        lead.LastUpdatedBy = team.LastUpdatedBy;

                        await _slackService.InviteMemberToChannelAsync(slackUser.User.Id, cancellationToken);
                    }
                }

                var content = new StringBuilder("You have been assigned to team ");
                content.AppendLine($"<b>{team.Name}</b>");
                await _graphEmailService.SendMailAsync(lead.Email, "Assigned To Team", content, cancellationToken);
            }
        }

        public async Task Handle(UpdatedEvent notification, CancellationToken cancellationToken)
        {
            var team = notification.GetEntity<Team>();
            if (team != null)
            {

            }
        }
    }
}
