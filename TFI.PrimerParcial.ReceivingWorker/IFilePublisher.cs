using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.FileProcessor
{
    public interface IFilePublisher
    {
        Task Publish(FileUploadInfo fileUpload);
    }
}