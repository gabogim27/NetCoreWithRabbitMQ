using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rabbit.Common.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Rabbit.Common
{
    public abstract class BaseProducer<T> : RabbitMqClientBase, IProducer<T>
    {
        private readonly ILogger<BaseProducer<T>> logger;
        protected abstract string ExchangeName { get; }
        protected abstract string RoutingKeyName { get; }
        protected abstract string AppId { get; }

        protected BaseProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqClientBase> logger,
            ILogger<BaseProducer<T>> producerBaseLogger) :
            base(connectionFactory, logger) => logger = producerBaseLogger;
        public void Publish(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                var properties = Channel.CreateBasicProperties();
                properties.AppId = AppId;
                properties.ContentType = "application/json";
                properties.DeliveryMode = 1; // Doesn't persist to disk
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                Channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body, basicProperties: properties);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error while publishing");
            }
        }
    }
}
