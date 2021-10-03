using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.FileConsumer.Printer
{
    public interface IPrinter
    {
        bool SendToPrint(FileUploadInfo file);
    }
}