using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IRabbitService<T> where T : class
    {
        Task SendToQueue(T data, string queue, int priority);
    }
}
