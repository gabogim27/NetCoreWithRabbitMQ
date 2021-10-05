using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.FileProcessor;
using TFI.PrimerParcial.RabbitCommon.Implementations;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class Program
    {
        private static IServiceProvider serviceProvider;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", false);
            var config = builder.Build();
            CreateHostBuilder(args, config).Build();
            var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<FileConsumer>().Consume();
            Console.WriteLine("Process finished");
            Console.ReadLine();
            DisposeServices();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPrinter, Printer>();
                    services.AddTransient<IFilePublisher, FilePublisher>();
                    services.AddSingleton<FileConsumer>();
                    services.AddTransient(typeof(IPublisher<>), typeof(Publisher<>));
                    services.AddTransient<IConsumer, Consumer>();
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