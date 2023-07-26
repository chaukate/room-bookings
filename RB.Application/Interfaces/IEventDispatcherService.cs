using MediatR;

namespace RB.Application.Interfaces
{
    public interface IEventDispatcherService
    {
        void QueueNotification(INotification notification);
        void ClearQueue();
        Task Dispatch(CancellationToken cancellationToken);
    }
}
