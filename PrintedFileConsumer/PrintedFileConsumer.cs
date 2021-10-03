using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace PrintedFileConsumer
{
    public class PrintedFileConsumer : IConsumer<FileUploadInfo>
    {
        private readonly IRepository<FileUploadInfo> repository;
        private readonly ILogger<PrintedFileConsumer> logger;

        public PrintedFileConsumer(IRepository<FileUploadInfo> repository, ILogger<PrintedFileConsumer> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<FileUploadInfo> context)
        {
            if (context != null)
            {
                logger.LogInformation($"Received file: {context.Message.FileName} to PrintedFileConsumer.");

                var data = context.Message;
                data.DatabaseUpdated = DateTime.Now;
                data.Id = Guid.NewGuid();
                repository.Add(data);
                logger.LogInformation("Saved to database");
            }

            return Task.CompletedTask;
        }
    }
}
