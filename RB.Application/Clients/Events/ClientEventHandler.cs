using MediatR;
using RB.Application.Common.Events;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using RB.Domain.Enumerations;

namespace RB.Application.Clients.Events
{
    public class ClientEventHandler : INotificationHandler<CreatedEvent>, INotificationHandler<UpdatedEvent>
    {
        private readonly IGraphAdminService _graphAdminService;
        public ClientEventHandler(IGraphAdminService graphAdminService)
        {
            _graphAdminService = graphAdminService;
        }

        public async Task Handle(CreatedEvent notification, CancellationToken cancellationToken)
        {
            var client = notification.GetEntity<Client>();
            if (client != null)
            {
                await _graphAdminService.RequestConsentAsync(client, cancellationToken);
            }
        }

        public async Task Handle(UpdatedEvent notification, CancellationToken cancellationToken)
        {
            var client = notification.GetEntity<Client>();
            if (client != null)
            {
                switch (client.Activity)
                {
                    case ClientActivity.RequestConsent:
                        await _graphAdminService.RequestConsentAsync(client, cancellationToken);
                        break;
                    case ClientActivity.RecordConsent:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
