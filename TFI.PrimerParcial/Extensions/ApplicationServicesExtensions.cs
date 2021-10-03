using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using TFI.PrimerParcial.Source.Repository.Implementations;
using TFI.PrimerParcial.Source.Repository.Interfaces;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IWorkerService<>), typeof(WorkerService<>));

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
