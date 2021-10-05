using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using TFI.PrimerParcial.RabbitCommon.Implementations;
using TFI.PrimerParcial.RabbitCommon.Interfaces;
using TFI.PrimerParcial.Source.Data;
using TFI.PrimerParcial.Source.Repository.Implementations;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace PrintedFileConsumer
{
    public class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", false);
            var config = builder.Build();
            CreateHostBuilder(args, config).Build();
            var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<PrintedFileConsumer>().Consume();
            Console.WriteLine("Process finished");
            Console.ReadLine();
            DisposeServices();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddDbContext<FileInfoDbContext>(o => o.UseSqlServer(config.GetConnectionString("FileInfoDbConnection")));
                    services.AddTransient(typeof(IPublisher<>), typeof(Publisher<>));
                    services.AddTransient<IConsumer, Consumer>();
                    services.AddScoped<PrintedFileConsumer>();
                    serviceProvider = services.BuildServiceProvider(true);
                });

        private static void DisposeServices()
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose();
            }
        }
    }
}
