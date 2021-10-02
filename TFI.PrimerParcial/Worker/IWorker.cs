using System.Threading.Tasks;
using TFI.PrimerParcial.Dtos;

namespace TFI.PrimerParcial.Worker
{
    public interface IWorker
    {
        Task sendToQueue(UploadFileDto uploadFileDto);
    }
}
