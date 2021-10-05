using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.FileProcessor;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer
    {
        private readonly ILogger<FileConsumer> logger;
        private readonly IPrinter printer;
        private readonly IFilePublisher publisher;
        private readonly IConsumer consumer;
        private readonly IConfiguration config;

        public FileConsumer(ILogger<FileConsumer> logger, IPrinter printer, IFilePublisher publisher, IConsumer consumer, IConfiguration config)
        {
            this.logger = logger;
            this.printer = printer;
            this.publisher = publisher;
            this.consumer = consumer;
            this.config = config;
        }

        public Task Consume()
        {
            var fileQueueString = config["RabbitMQ:FileQueue"];
            var fileQueue = fileQueueString.Substring(fileQueueString.LastIndexOf('/') + 1);
            var ctxs = consumer.ConsumeMessage(fileQueue, config["RabbitConnString:connStr"]);
            if (ctxs?.Count > 0)
            {
                logger.LogInformation($"Consuming from queue: {fileQueue}.");

                ctxs.ForEach(doc => 
                {
                    var obj = JsonConvert.DeserializeObject<UploadFileDto>(doc);
                    logger.LogInformation($"Consuming file: {obj.FileName} with priority: {obj.Priority}.");

                    var fileUpload = new FileUploadInfo()
                    {
                        FileName = obj.FileName,
                        Priority = obj.Priority
                    };

                    var result = printer.SendToPrint(fileUpload);
                    if (result)
                    {
                        var dbQueueString = config["RabbitMQ:DatabaseQueue"];
                        var dbQueue = dbQueueString.Substring(dbQueueString.LastIndexOf('/') + 1);
                        logger.LogInformation($"Going to send file: {obj.FileName} to queue: {dbQueue}.");
                        publisher.Publish(fileUpload);
                    }
                });
            }

            return Task.CompletedTask;
        }
    }
}