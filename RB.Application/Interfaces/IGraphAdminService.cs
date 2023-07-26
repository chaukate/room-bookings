namespace RB.Application.Interfaces
{
    public interface IGraphAdminService
    {
        Task SendAdminEmailAsync(string recipent, string subject, string content, CancellationToken cancellationToken);
    }
}
