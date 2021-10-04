using Entities;

namespace Processor.Printer.Contracts
{
    public interface IPrinter
    {
        bool SendToPrint(ConsumedFile file);
    }
}