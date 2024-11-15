using App.Domain.Events;
namespace App.Application.Contracts.ServiceBus
{
    public interface IServiceBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellation = default) where T : IMessage, IEvent;

        Task SendAsync<T>(T message, CancellationToken cancellation = default) where T : IMessage, IEvent;
    }
}
