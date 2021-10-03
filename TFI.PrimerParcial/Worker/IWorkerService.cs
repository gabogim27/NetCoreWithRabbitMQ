using System.Threading.Tasks;

namespace TFI.PrimerParcial.Worker
{
    public interface IWorkerService<T> where T : class
    {
        Task SendToQueue(T data, string queue);
    }
}
