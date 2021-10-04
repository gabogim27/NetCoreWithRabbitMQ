using System.Threading.Tasks;
using Entities;

namespace Services.Contracts
{
    public interface IRabbitService<T> where T : class
    {
        Task SendToQueue(T data, string queue, int priority);

        File ConsumeFromQueue(string queue);
    }
}
