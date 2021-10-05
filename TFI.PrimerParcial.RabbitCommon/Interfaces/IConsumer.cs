using System.Collections.Generic;

namespace TFI.PrimerParcial.RabbitCommon.Interfaces
{
    public interface IConsumer
    {
        List<string> ConsumeMessage(string queue, string connString);
    }
}
