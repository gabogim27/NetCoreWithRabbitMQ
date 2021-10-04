using System.Threading.Tasks;
using Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Processor.Contracts;
using Processor.Printer.Contracts;

namespace Processor
{
    public class Consumer : IConsumer<File>
    {
        private readonly ILogger<Consumer> logger;
        private readonly IPrinter printer;
        private readonly IPublisher publisher;

        public Consumer(
            ILogger<Consumer> logger,
            IPrinter printer,
            IPublisher publisher)
        {
            this.logger = logger;
            this.printer = printer;
            this.publisher = publisher;
        }

        public Task Consume(ConsumeContext<File> context)
        {
            logger.LogInformation($"Start consuming file {context.Message.FileName}");

            var recivedData = context.Message;

            var consumedFile = new ConsumedFile() { Priority = recivedData.Priority, FileName = recivedData.FileName };

            var result = printer.SendToPrint(consumedFile);

            if (result)
            {
                logger.LogInformation($"Sending data in dbQueue");
                publisher.Publish(consumedFile);
            }

            return Task.CompletedTask;
        }
    }
}