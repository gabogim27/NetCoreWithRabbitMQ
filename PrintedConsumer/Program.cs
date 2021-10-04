using System;
using System.IO;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;

namespace PrintedFileConsumer
{
    public static class Program
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
                    services.AddDbContext<PrintDbContext>(o => o.UseSqlServer(config.GetConnectionString("TFIDbConnection")));
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<PrintedConsumer>();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host(new Uri("rabbitmq://localhost"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ReceiveEndpoint("dbQueue", ep =>
                            {
                                ep.UseMessageRetry(r => r.Interval(2, 100));
                                ep.ConfigureConsumer<PrintedConsumer>(provider);
                            });
                        }));
                    });
                    services.AddMassTransitHostedService();
                });
    }
}
