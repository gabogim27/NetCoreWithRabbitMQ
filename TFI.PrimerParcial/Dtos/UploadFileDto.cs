using System;
using System.Collections.Generic;

namespace TFI.PrimerParcial.Dtos
{
    public class UploadFileDto
    {
        public int Priority { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        //public IList<byte[]> Files { get; set; }
    }
}
