using App.Domain.Events;
using System.Diagnostics.Tracing;
namespace App.Application.Contracts.ServiceBus
{
    public interface IServiceBus
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellation = default) where T : IMessage, IEvent;

        Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default) where T : IMessage, IEvent;
    }
}
