using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.RabbitCommon.Interfaces;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace PrintedFileConsumer
{
    public class PrintedFileConsumer
    {
        private readonly IRepository<FileUploadInfo> repository;
        private readonly ILogger<PrintedFileConsumer> logger;
        private readonly IConsumer consumer;
        private readonly IConfiguration config;

        public PrintedFileConsumer(IRepository<FileUploadInfo> repository, ILogger<PrintedFileConsumer> logger, IConsumer consumer, IConfiguration config)
        {
            this.repository = repository;
            this.logger = logger;
            this.consumer = consumer;
            this.config = config;
        }

        public Task Consume()
        {
            var dbQueueString = config["RabbitMQ:DatabaseQueue"];
            var dbQueue = dbQueueString.Substring(dbQueueString.LastIndexOf('/') + 1);
            var ctxs = consumer.ConsumeMessage(dbQueue, config["RabbitConnString:connStr"]);
            
            if (ctxs?.Count > 0)
            {
                ctxs.ForEach(x => 
                {
                    var data = JsonConvert.DeserializeObject<FileUploadInfo>(x);
                    logger.LogInformation($"Preparing to store in database. {data.FileName}");
                    data.DatabaseUpdated = DateTime.Now;
                    data.Id = Guid.NewGuid();
                    repository.Add(data);
                    logger.LogInformation("Saved to database");
                });
            }

            return Task.CompletedTask;
        }
    }
}
