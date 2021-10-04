using System;
using Entities;
using Processor.Printer.Contracts;

namespace Processor.Printer
{
    public class Printer : IPrinter
    {
        public bool SendToPrint(ConsumedFile file)
        {
            file.PrintDate = DateTime.Now;

            return new Random().NextDouble() > 0.5;
        }
    }
}
