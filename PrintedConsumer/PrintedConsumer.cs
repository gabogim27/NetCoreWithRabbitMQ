using System;
using System.Threading.Tasks;
using Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Interfaces;
using Services.Contracts;

namespace PrintedFileConsumer
{
    public class PrintedConsumer
    {
        private readonly IRabbitService<ConsumedFile> rabbitService;
        private readonly IRepository<ConsumedFile> repository;
        private readonly ILogger<PrintedConsumer> logger;

        public PrintedConsumer(IRepository<ConsumedFile> repository, ILogger<PrintedConsumer> logger, IRabbitService<ConsumedFile> rabbitService)
        {
            this.repository = repository;
            this.logger = logger;
            this.rabbitService = rabbitService;
        }

        public Task Consume()
        {
            var context = rabbitService.ConsumeFromQueue("dbQueue");

            foreach (var serializedItem in context)
            {
                var item = JsonConvert.DeserializeObject<ConsumedFile>(serializedItem);

                logger.LogInformation($"The file {item.FileName} will be stored in database");

                item.DatabaseUpdated = DateTime.Now;
                item.Id = Guid.NewGuid();
                repository.Add(item);

                logger.LogInformation("Finished. Saved to database");
            }

            return Task.CompletedTask;
        }
    }
}
