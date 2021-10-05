using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

namespace TFI.PrimerParcial.FileProcessor
{
    public class FilePublisher : IFilePublisher
    {
        private readonly IPublisher<FileUploadInfo> publisher;
        private readonly IConfiguration config;

        public FilePublisher(IPublisher<FileUploadInfo> publisher, IConfiguration config)
        {
            this.publisher = publisher;
            this.config = config;
        }

        public Task Publish(FileUploadInfo fileUpload)
        {
            var dbQueueString = config["RabbitMQ:DatabaseQueue"];
            var dbQueue = dbQueueString.Substring(dbQueueString.LastIndexOf('/') + 1);
            publisher.SendToQueue(fileUpload, dbQueue, config["RabbitConnString:connStr"], fileUpload.Priority);
            return Task.CompletedTask;
        }
    }
}
