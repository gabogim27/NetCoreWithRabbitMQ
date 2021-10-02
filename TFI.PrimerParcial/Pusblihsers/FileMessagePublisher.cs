using Microsoft.Extensions.Logging;
using Rabbit.Common;
using RabbitMQ.Client;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.Pusblihsers
{
    public class FileMessagePublisher : BaseProducer<FileProperties>
    {
        public FileMessagePublisher(ConnectionFactory connectionFactory,
        ILogger<RabbitMqClientBase> logger,
        ILogger<BaseProducer<FileProperties>> producerBaseLogger) :
        base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CUSTOM_HOST.LoggerExchange";

        protected override string RoutingKeyName => "log.message";

        protected override string AppId => "LogProducer";
    }
}
