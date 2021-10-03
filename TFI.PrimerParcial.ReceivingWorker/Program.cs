using System;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.Worker;
using TFI.PrimerParcial.FileProcessor;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPrinter, Printer>();
                    services.AddTransient<IFilePublisher, FilePublisher>();
                    services.AddTransient(typeof(IWorkerService<>), typeof(WorkerService<>));
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FileConsumer>();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri("rabbitmq://localhost"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            config.ReceiveEndpoint("fileQueue", ep =>
                            {
                                ep.PrefetchCount = 16;
                                ep.UseMessageRetry(r => r.Interval(2, 100));
                                ep.ConfigureConsumer<FileConsumer>(provider);
                            });
                        }));
                    });
                    services.AddMassTransitHostedService();
                });
    }
}