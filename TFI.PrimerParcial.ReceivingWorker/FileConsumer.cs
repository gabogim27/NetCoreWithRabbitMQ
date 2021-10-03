using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.FileProcessor;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer : IConsumer<UploadFileDto>
    {
        private readonly ILogger<FileConsumer> logger;
        private readonly IPrinter printer;
        private readonly IFilePublisher publisher;

        public FileConsumer(ILogger<FileConsumer> logger, IPrinter printer, IFilePublisher publisher)
        {
            this.logger = logger;
            this.printer = printer;
            this.publisher = publisher;
        }

        public Task Consume(ConsumeContext<UploadFileDto> context)
        {
            if (context != null)
            {
                logger.LogInformation($"Received file: {context.Message.FileName} to FileConsumer.");

                var data = context.Message;

                var fileUpload = new FileUploadInfo()
                {
                    FileName = data.FileName,
                    Priority = data.Priority
                };

                var result = printer.SendToPrint(fileUpload);

                if (result)
                {
                    logger.LogInformation($"Sending data in databaseQueue, priority {fileUpload.Priority}");
                    publisher.Publish(fileUpload);
                }
            }

            return Task.CompletedTask;
        }
    }
}