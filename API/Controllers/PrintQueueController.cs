using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using Services.Contracts;

namespace TFI.PrimerParcial.Controllers
{
    public class PrintQueueController : BaseApiController
    {
        private readonly IRepository<ConsumedFile> repository;
        private readonly IRabbitService<File> rabbit;
        private readonly IConfiguration config;

        public PrintQueueController(
            IRepository<ConsumedFile> repository,
            IRabbitService<File> rabbit,
            IConfiguration config)
        {
            this.repository = repository;
            this.rabbit = rabbit;
            this.config = config;
        }

        [HttpPost("SendFiles")]
        public async Task<ActionResult<bool>> SendFiles([FromBody] File file)
        {
            if (file != null && file.Priority > 0 && !string.IsNullOrWhiteSpace(file.FileName))
            {
                await rabbit.SendToQueue(file, config["RabbitMQ:FileQueue"], file.Priority);

                return Ok(file);
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<bool> GetDocument(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileData = repository.GetByName(fileName);

                if (fileData == null)
                {
                    return Ok("Fallo en la impresion");
                }

                return Ok(fileData);
            }

            return BadRequest();
        }
    }
}
