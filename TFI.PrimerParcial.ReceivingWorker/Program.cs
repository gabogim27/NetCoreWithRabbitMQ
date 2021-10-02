using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace TFI.PrimerParcial.ReceivingWorker
{
    class Program
    {
        public static void Main(string[] args)
        {
            //ver como inicializar el consumer en una app de consola
        }

    private static void Initialize(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ServiceBusConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("ticketQueue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        //ep.UseMessageRetry(r => r.ConnectRetryObserver());
                        ep.ConfigureConsumer<ServiceBusConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
        }
    }
}
