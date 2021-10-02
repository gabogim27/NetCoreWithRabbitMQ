using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.Source.Repository.Interfaces;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IRepository<FileUploadInfo> repository;
        private readonly IWorker worker;

        public FileController(IRepository<FileUploadInfo> repository, IWorker worker)
        {
            this.repository = repository;
            this.worker = worker;
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

                await worker.sendToQueue(uploadFileDto);

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
    }
}
