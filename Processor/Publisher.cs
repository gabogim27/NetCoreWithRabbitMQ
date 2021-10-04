using System.Threading.Tasks;
using Entities;
using Microsoft.Extensions.Configuration;
using Processor.Contracts;
using Services.Contracts;

namespace Processor
{
    public class Publisher : IPublisher
    {
        private readonly IRabbitService<ConsumedFile> rabbit;
        private readonly IConfiguration config;

        public Publisher(IRabbitService<ConsumedFile> rabbit, IConfiguration config)
        {
            this.rabbit = rabbit;
            this.config = config;
        }

        public Task Publish(ConsumedFile consumedFile)
        {
            rabbit.SendToQueue(consumedFile, config["RabbitMQ:DatabaseQueue"], consumedFile.Priority);

            return Task.CompletedTask;
        }
    }
}
