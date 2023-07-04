using RB.Application.Common.Models;

namespace RB.Application.Interfaces
{
    public interface ISlackService
    {
        Task SendMessageAsync(string message, CancellationToken cancellationToken);
        Task<SlackUserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task InviteMemberToChannelAsync(string slackUserId, CancellationToken cancellationToken);
        Task RemoveMemberFromChannelAsync(string slackUserId, CancellationToken cancellationToken);
    }
}
