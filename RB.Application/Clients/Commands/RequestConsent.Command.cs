using MediatR;
using RB.Application.Common.Exceptions;
using RB.Application.Common.Helpers;
using RB.Application.Interfaces;
using RB.Domain.Enumerations;

namespace RB.Application.Clients.Commands
{
    public class RequestConsentHandler : IRequestHandler<RequestConsentCommand, bool>
    {
        private readonly IRBDbContext _dbContext;
        public RequestConsentHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(RequestConsentCommand request, CancellationToken cancellationToken)
        {
            var dbClient = await _dbContext.Clients.FindAsync(request.ClientId, cancellationToken);
            if (dbClient == null)
                throw new NotFoundException();

            dbClient.SecretKey = StringHelper.RandomString(100);
            dbClient.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbClient.LastUpdatedBy = request.CurrentUser;
            dbClient.Activity = ClientActivity.RequestConsent;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    public class RequestConsentCommand : IRequest<bool>
    {
        public int ClientId { get; set; }
        public string CurrentUser { get; set; }
    }
}
