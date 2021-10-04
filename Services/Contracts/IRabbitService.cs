using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IRabbitService<T> where T : class
    {
        Task SendToQueue(T data, string queue, int priority);

        List<string> ConsumeFromQueue(string queue);
    }
}
