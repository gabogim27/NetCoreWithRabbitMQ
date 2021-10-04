using System.Threading.Tasks;
using Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Processor.Contracts;
using Processor.Printer.Contracts;
using Services.Contracts;

namespace Processor
{
    public class Consumer
    {
        private readonly ILogger<Consumer> logger;
        private readonly IPrinter printer;
        private readonly IPublisher publisher;
        private readonly IRabbitService<File> rabbitService;

        public Consumer(
            ILogger<Consumer> logger,
            IPrinter printer,
            IPublisher publisher,
            IRabbitService<File> rabbitService)
        {
            this.logger = logger;
            this.printer = printer;
            this.publisher = publisher;
            this.rabbitService = rabbitService;
        }

        public Task Consume()
        {
            var context = rabbitService.ConsumeFromQueue("fileQueue");

            foreach (var serializedItem in context)
            {
                var item = JsonConvert.DeserializeObject<File>(serializedItem);

                logger.LogInformation($"Start consuming file {item.FileName} priority: {item.Priority}");

                var consumedFile = new ConsumedFile() { Priority = item.Priority, FileName = item.FileName };

                var result = printer.SendToPrint(consumedFile);

                if (result)
                {
                    logger.LogInformation($"Sending data in dbQueue");
                    publisher.Publish(consumedFile);
                }
            }
            return Task.CompletedTask;
        }
    }
}