﻿using System;
using System.Threading.Tasks;
using MassTransit;

namespace TFI.PrimerParcial.Worker
{
    public class WorkerService<T> : IWorkerService<T> where T : class
    {
        private readonly IBus bus;

        public WorkerService(IBus bus)
        {
            this.bus = bus;
        }

        public async Task SendToQueue(T data, string queue, int priority)
        {
            var uri = new Uri(queue);
            var endpoint = await bus.GetSendEndpoint(uri);

            await endpoint.Send(data, context => context.SetPriority((byte)priority));
        }
    }
}
