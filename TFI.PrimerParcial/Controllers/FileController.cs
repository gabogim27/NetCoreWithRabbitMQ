﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TFI.PrimerParcial.Domain;
using TFI.PrimerParcial.Dtos;
using TFI.PrimerParcial.Source.Repository.Interfaces;
using TFI.PrimerParcial.Worker;

namespace TFI.PrimerParcial.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IRepository<FileUploadInfo> repository;
        private readonly IWorkerService<UploadFileDto> worker;
        private readonly IConfiguration config;

        public FileController(IRepository<FileUploadInfo> repository, IWorkerService<UploadFileDto> worker, IConfiguration config)
        {
            this.repository = repository;
            this.worker = worker;
            this.config = config;
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

                await worker.SendToQueue(uploadFileDto, config["RabbitMQ:FileQueue"], uploadFileDto.Priority);

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
