using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;
using Services;
using Services.Contracts;

namespace PrintedFileConsumer
{
    public static class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", false);
            var config = builder.Build();
            CreateHostBuilder(args, config).Build();

            var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<PrintedConsumer>().Consume();
            Console.WriteLine("Process finished");
            Console.ReadLine();
            DisposeServices();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddDbContext<PrintDbContext>(o => o.UseSqlServer(config.GetConnectionString("TFIDbConnection")));
                    services.AddTransient(typeof(IRabbitService<>), typeof(RabbitService<>));
                    services.AddScoped<PrintedConsumer>();
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
