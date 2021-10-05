using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.FileProcessor;
using TFI.PrimerParcial.RabbitCommon.Implementations;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

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
                    services.AddTransient<FileConsumer>();
                    services.AddTransient(typeof(IPublisher<>), typeof(Publisher<>));
                    services.AddTransient<IConsumer, Consumer>();
                });
    }
}