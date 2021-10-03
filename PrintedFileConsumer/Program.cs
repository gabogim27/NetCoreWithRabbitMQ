using System;
using System.IO;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFI.PrimerParcial.Source.Data;
using TFI.PrimerParcial.Source.Repository.Implementations;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace PrintedFileConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", false);
            var config = builder.Build();
            CreateHostBuilder(args, config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddDbContext<FileInfoDbContext>(o => o.UseSqlServer(config.GetConnectionString("FileInfoDbConnection")));
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<PrintedFileConsumer>();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host(new Uri("rabbitmq://localhost"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ReceiveEndpoint("databaseQueue", ep =>
                            {
                                ep.EnablePriority((byte)10);
                                ep.PrefetchCount = 10;
                                ep.UseMessageRetry(r => r.Interval(2, 100));
                                ep.ConfigureConsumer<PrintedFileConsumer>(provider);
                            });
                        }));
                    });
                    services.AddMassTransitHostedService();
                });
    }
}
