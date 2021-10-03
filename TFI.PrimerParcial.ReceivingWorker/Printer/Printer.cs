using System;
using TFI.PrimerParcial.Domain;

namespace TFI.PrimerParcial.FileConsumer.Printer
{
    public class Printer : IPrinter
    {
        public bool SendToPrint(FileUploadInfo file)
        {
            file.PrintDate = DateTime.Now;

            return new Random().NextDouble() > 0.5;
        }
    }
}
