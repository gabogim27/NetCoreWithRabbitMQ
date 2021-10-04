using System.Threading.Tasks;
using Entities;

namespace Processor.Contracts
{
    public interface IPublisher
    {
        Task Publish(ConsumedFile fileUpload);
    }
}