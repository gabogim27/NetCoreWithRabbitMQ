using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer : IConsumer<FileUploadInfo>
    {
        readonly ILogger<FileConsumer> logger;

        public FileConsumer(ILogger<FileConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<FileUploadInfo> context)
        {
            logger.LogInformation($"Received file: {context.Message.FileName}");

            var data = context.Message;

            return Task.CompletedTask;
        }
    }
}