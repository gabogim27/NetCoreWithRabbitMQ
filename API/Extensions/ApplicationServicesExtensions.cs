using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Interfaces;
using Services;
using Services.Contracts;

namespace TFI.PrimerParcial.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRabbitService<>), typeof(RabbitService<>));

            return services;
        }
    }
}
