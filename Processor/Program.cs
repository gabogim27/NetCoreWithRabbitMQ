using System;
using GreenPipes;
using MassTransit;
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
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPrinter, Printer.Printer>();
                    services.AddTransient<IPublisher, Publisher>();
                    services.AddTransient(typeof(IRabbitService<>), typeof(RabbitService<>));
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<Consumer>();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri("rabbitmq://localhost"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            config.ReceiveEndpoint("fileQueue", ep =>
                            {
                                ep.EnablePriority(10);
                                ep.UseMessageRetry(r => r.Interval(2, 100));
                                ep.ConfigureConsumer<Consumer>(provider);
                            });
                        }));
                    });
                    services.AddMassTransitHostedService();
                });
    }
}