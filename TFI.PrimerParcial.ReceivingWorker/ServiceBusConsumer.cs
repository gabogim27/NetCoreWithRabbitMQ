using MassTransit;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.ReceivingWorker
{
    public class ServiceBusConsumer : IConsumer<FileUploadInfo>
    {
        public async Task Consume(ConsumeContext<FileUploadInfo> context)
        {
            var data = context.Message;
        }
    }
}