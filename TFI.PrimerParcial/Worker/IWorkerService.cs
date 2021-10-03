using System.Threading.Tasks;
using TFI.PrimerParcial.Dtos;

namespace TFI.PrimerParcial.Worker
{
    public interface IWorkerService
    {
        Task SendToQueue(UploadFileDto uploadFileDto);
    }
}
