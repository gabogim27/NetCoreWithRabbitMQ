using System;

namespace Entities
{
    public class ConsumedFile
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public DateTime PrintDate { get; set; }

        public DateTime DatabaseUpdated { get; set; }

        public int Priority { get; set; }
    }
}
