using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.RabbitCommon.Interfaces;
using TFI.PrimerParcial.Source.Repository.Interfaces;
using Microsoft.Extensions.Logging;


namespace TFI.PrimerParcial.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IRepository<FileUploadInfo> repository;
        private readonly IPublisher<UploadFileDto> publisher;
        private readonly IConfiguration config;
        private readonly ILogger<FileController> logger;

        public FileController(IRepository<FileUploadInfo> repository, IPublisher<UploadFileDto> publisher, IConfiguration config, ILogger<FileController> logger)
        {
            this.repository = repository;
            this.publisher = publisher;
            this.config = config;
            this.logger = logger;
        }

        [HttpPost("UploadFiles")]
        public async Task<ActionResult<bool>> UploadFiles([FromBody] UploadFileDto uploadFileDto)
        {
            try
            {
                if (uploadFileDto == null || uploadFileDto.Priority <= 0 || string.IsNullOrWhiteSpace(uploadFileDto.FileName) )
                {
                    return BadRequest();
                }

                var queue = config["RabbitMQ:FileQueue"]; 
                await publisher.SendToQueue(uploadFileDto, queue.Substring(queue.LastIndexOf('/') + 1), config["RabbitConnString:connStr"], uploadFileDto.Priority);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured:", ex);
                return false;
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
                logger.LogError("An error occured:", ex);
                return false;
            }
        }
    }
}
