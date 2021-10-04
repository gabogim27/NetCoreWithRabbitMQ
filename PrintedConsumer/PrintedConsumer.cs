using System;
using System.Threading.Tasks;
using Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;

namespace PrintedFileConsumer
{
    public class PrintedConsumer : IConsumer<ConsumedFile>
    {
        private readonly IRepository<ConsumedFile> repository;
        private readonly ILogger<PrintedConsumer> logger;

        public PrintedConsumer(IRepository<ConsumedFile> repository, ILogger<PrintedConsumer> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<ConsumedFile> context)
        {
            if (context != null)
            {
                logger.LogInformation($"The file {context.Message.FileName} will be stored in database");

                var data = context.Message;

                data.DatabaseUpdated = DateTime.Now;
                data.Id = Guid.NewGuid();
                repository.Add(data);

                logger.LogInformation("Finished. Saved to database");
            }

            return Task.CompletedTask;
        }
    }
}
