using App.Application.Contracts.ServiceBus;
using App.Domain.Events;

namespace App.Bus
{
    public class ServiceBus : IServiceBus
    {
        public Task PublishAsync<T>(T message, CancellationToken cancellation = default) where T : IMessage, IEvent
        {
            throw new NotImplementedException();
        }

        public Task SendAsync<T>(T message, CancellationToken cancellation = default) where T : IMessage, IEvent
        {
            throw new NotImplementedException();
        }
    }
}
