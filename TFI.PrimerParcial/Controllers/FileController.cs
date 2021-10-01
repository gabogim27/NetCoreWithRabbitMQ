using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Dtos;

namespace TFI.PrimerParcial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseApiController
    {
        [HttpPost("Upload")]
        public async Task<ActionResult<bool>> UploadFiles(UploadFileDto uploadFileDto)
        {
            try
            {
                if (uploadFileDto == null || uploadFileDto.Priority <= 0 || uploadFileDto.Files.Count == 0)
                {
                    return BadRequest();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
