using System.Threading.Tasks;

namespace TFI.PrimerParcial.RabbitCommon.Interfaces
{
    public interface IPublisher<T>
    {
        Task SendToQueue(T value, string queue, string connString, int? priority = null);
    }
}
