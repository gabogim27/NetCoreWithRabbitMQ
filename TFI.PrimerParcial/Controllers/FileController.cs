using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TFI.PrimerParcial.Dtos;

namespace TFI.PrimerParcial.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IBus bus;
        private readonly IConfiguration config;

        public FileController(IBus bus, IConfiguration config)
        {
            this.bus = bus;
            this.config = config;
        }

        [HttpPost("UploadFiles")]
        public async Task<ActionResult<bool>> UploadFiles(UploadFileDto uploadFileDto)
        {
            try
            {
                if (uploadFileDto == null || uploadFileDto.Priority <= 0 || uploadFileDto.Files == null || uploadFileDto.Files.Count == 0)
                {
                    return BadRequest();
                }

                await sendToQueue(uploadFileDto);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task sendToQueue(UploadFileDto uploadFileDto)
        {
            var uri = new Uri(config["RabbitMQ:FileQueue"]);
            var endpoint = await bus.GetSendEndpoint(uri);
            await endpoint.Send(uploadFileDto);
        }
    }
}
