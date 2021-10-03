using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.FileConsumer.Printer;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer : IConsumer<FileUploadInfo>
    {
        private readonly ILogger<FileConsumer> logger;
        private readonly IPrinter printer;

        public FileConsumer(ILogger<FileConsumer> logger, IPrinter printer)
        {
            this.logger = logger;
            this.printer = printer;
        }

        public Task Consume(ConsumeContext<FileUploadInfo> context)
        {
            if (context != null)
            {
                logger.LogInformation($"Received file: {context.Message.FileName}");

                var data = context.Message;

                var result = printer.SendToPrint();

                if (result)
                {
                    data.Status = "Ok";
                    data.PrintDate = DateTime.Now;
                }
            }

            return Task.CompletedTask;
        }
    }
}