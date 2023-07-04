namespace RB.Application.Interfaces
{
    public interface ISlackService
    {
        Task SendMessageAsync(string message, CancellationToken cancellationToken);
    }
}
