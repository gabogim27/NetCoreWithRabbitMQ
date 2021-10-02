using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.Source.Repository.Interfaces;

namespace TFI.PrimerParcial.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IBus bus;
        private readonly IConfiguration config;
        private readonly IRepository<FileUploadInfo> repository;

        public FileController(IBus bus, IConfiguration config, IRepository<FileUploadInfo> repository)
        {
            this.bus = bus;
            this.config = config;
            this.repository = repository;
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
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult<bool> GetDocumentStatus(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest();
                }

                var fileData = repository.GetByName(fileName);

                if (fileData == null)
                {
                    return Ok("Fallo en la impresion");
                }

                return Ok(fileData);
            }
            catch (Exception ex)
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
