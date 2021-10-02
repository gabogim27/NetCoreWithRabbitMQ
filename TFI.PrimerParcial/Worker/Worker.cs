using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class Worker : IWorker
    {
        private readonly IBus bus;
        private readonly IConfiguration config;

        public Worker(IBus bus, IConfiguration config)
        {
            this.bus = bus;
            this.config = config;
        }

        public async Task sendToQueue(UploadFileDto uploadFileDto)
        {
            var uri = new Uri(config["RabbitMQ:FileQueue"]);
            var endpoint = await bus.GetSendEndpoint(uri);
            await endpoint.Send(uploadFileDto);
        }
    }
}
