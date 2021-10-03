using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Dtos;

namespace TFI.PrimerParcial.Worker
{
    public class WorkerService : IWorkerService
    {
        private readonly IBus bus;
        private readonly IConfiguration config;

        public WorkerService(IBus bus, IConfiguration config)
        {
            this.bus = bus;
            this.config = config;
        }

        public async Task SendToQueue(UploadFileDto uploadFileDto)
        {
            var uri = new Uri(config["RabbitMQ:FileQueue"]);
            var endpoint = await bus.GetSendEndpoint(uri);
            await endpoint.Send(uploadFileDto);
        }
    }
}
