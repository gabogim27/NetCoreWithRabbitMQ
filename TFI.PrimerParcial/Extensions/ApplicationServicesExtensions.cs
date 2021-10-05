using Microsoft.Extensions.DependencyInjection;
using TFI.PrimerParcial.RabbitCommon.Implementations;
using TFI.PrimerParcial.RabbitCommon.Interfaces;
using TFI.PrimerParcial.Source.Repository.Implementations;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace TFI.PrimerParcial.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IConsumer, Consumer>();
            services.AddTransient(typeof(IPublisher<>), typeof(Publisher<>));

            return services;
        }
    }
}
