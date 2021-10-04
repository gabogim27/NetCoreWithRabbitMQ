using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Processor.Contracts;
using Processor.Printer.Contracts;
using Services;
using Services.Contracts;

namespace Processor
{
    public static class Program
    {
        private static IServiceProvider serviceProvider;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build();
            var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<Consumer>().Consume();
            DisposeServices();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPrinter, Printer.Printer>();
                    services.AddTransient<IPublisher, Publisher>();
                    services.AddTransient(typeof(IRabbitService<>), typeof(RabbitService<>));
                    services.AddSingleton<Consumer>();

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