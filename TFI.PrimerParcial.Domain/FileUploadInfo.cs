using System;

namespace TFI.PrimerParcial.Domain
{
    public class FileUploadInfo
    {
        public Guid Id { get; set; } = new Guid();

        public string FileName { get; set; }

        public DateTime PrintDate { get; set; }

        public DateTime DatabaseUpdated { get; set; }
    }
}
