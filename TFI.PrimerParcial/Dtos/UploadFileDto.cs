using System;
using System.Collections.Generic;

namespace TFI.PrimerParcial.Dtos
{
    public class UploadFileDto
    {
        public int Priority { get; set; }

        public List<Byte[]> Files { get; set; }
    }
}
