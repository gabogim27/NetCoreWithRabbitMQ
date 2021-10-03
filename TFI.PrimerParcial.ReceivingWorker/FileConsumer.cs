using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer : IConsumer<UploadFileDto>
    {
        private readonly ILogger<FileConsumer> logger;
        private readonly IPrinter printer;
        private readonly IWorkerService<FileUploadInfo> worker;
        private readonly IConfiguration config;

        public FileConsumer(ILogger<FileConsumer> logger, IPrinter printer, IWorkerService<FileUploadInfo> worker, IConfiguration config)
        {
            this.logger = logger;
            this.printer = printer;
            this.worker = worker;
            this.config = config;
        }

        public Task Consume(ConsumeContext<UploadFileDto> context)
        {
            if (context != null)
            {
                logger.LogInformation($"Received file: {context.Message.FileName}");

                var data = context.Message;

                var fileUpload = new FileUploadInfo()
                {
                    FileName = data.FileName
                };

                var result = printer.SendToPrint(fileUpload);

                if (result)
                {
                    worker.SendToQueue(fileUpload, config["RabbitMQ:DatabaseQueue"]);
                }
            }

            return Task.CompletedTask;
        }
    }
}