using App.Bus.Consumers;
using App.Domain.Events;
using App.Domain.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Bus
{
    public static class BusExtensions
    {
        public static void AddBusExt(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusOption = configuration.GetSection(nameof(ServiceBusOptions)).Get<ServiceBusOptions>();

            services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
            {

                x.AddConsumer<ProductAddedEventConsumer>();

                cfg.Host(new Uri(serviceBusOption!.Url), h => { });
            }));
        }
    }
}
