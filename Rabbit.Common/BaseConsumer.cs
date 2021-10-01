using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Common
{
    public abstract class BaseConsumer : RabbitMqClientBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<BaseConsumer> logger;
        protected abstract string QueueName { get; }

        public BaseConsumer(IMediator mediator, ConnectionFactory connectionFactory, ILogger<BaseConsumer> consumerLogger, ILogger<RabbitMqClientBase> logger) :
            base(connectionFactory, logger)
        {
            this.mediator = mediator;
            this.logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = JsonConvert.DeserializeObject<T>(body);

                await mediator.Send(message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}
