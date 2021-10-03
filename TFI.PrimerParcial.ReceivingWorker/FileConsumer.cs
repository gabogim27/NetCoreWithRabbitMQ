using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.FileConsumer.Printer;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class FileConsumer : IConsumer<UploadFileDto>
    {
        private readonly ILogger<FileConsumer> logger;
        //private readonly IPrinter printer;
        //private readonly IWorkerService worker;

        public FileConsumer(ILogger<FileConsumer> logger)
        {
            this.logger = logger;
            //this.printer = printer;
            //this.worker = worker;
        }

        public Task Consume(ConsumeContext<UploadFileDto> context)
        {
            if (context != null)
            {
                logger.LogInformation($"Received file: {context.Message.FileName}");

                var data = context.Message;

                //var result = printer.SendToPrint(data);

                //data.Status = result ? FileUploadInfo.PrintStatus.Ok : FileUploadInfo.PrintStatus.Failed;

                //worker.SendToQueue(data);
            }

            return Task.CompletedTask;
        }
    }
}