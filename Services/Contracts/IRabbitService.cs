using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Contracts
{
    public interface IRabbitService<T> where T : class
    {
        Task SendToQueue(T data, string queue, int priority);

        List<File> ConsumeFromQueue(string queue);
    }
}
