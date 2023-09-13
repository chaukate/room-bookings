using RB.Domain.Entities;

namespace RB.Application.Interfaces
{
    public interface IGraphAdminService
    {
        Task RequestConsentAsync(Client client, CancellationToken cancellationToken);
        Task SendAdminEmailAsync(string recipent, string subject, string content, CancellationToken cancellationToken);
    }
}
