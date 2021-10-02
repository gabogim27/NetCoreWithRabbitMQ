using System;

namespace TFI.PrimerParcial.FileConsumer.Printer
{
    public class Printer : IPrinter
    {
        public bool SendToPrint()
        {
            return new Random().NextDouble() > 0.5;
        }
    }
}
