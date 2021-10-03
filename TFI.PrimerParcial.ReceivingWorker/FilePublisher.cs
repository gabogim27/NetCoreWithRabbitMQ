using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.FileProcessor
{
    public class FilePublisher : IFilePublisher
    {
        private readonly IWorkerService<FileUploadInfo> worker;
        private readonly IConfiguration config;

        public FilePublisher(IWorkerService<FileUploadInfo> worker, IConfiguration config)
        {
            this.worker = worker;
            this.config = config;
        }

        public Task Publish(FileUploadInfo fileUpload)
        {
            worker.SendToQueue(fileUpload, config["RabbitMQ:DatabaseQueue"]);

            return Task.CompletedTask;
        }
    }
}
