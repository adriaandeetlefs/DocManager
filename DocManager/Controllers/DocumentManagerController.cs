using DocManager.DAL.Entities;
using DocManager.DAL.Interfaces;
using DocManager.Helpers.Extensions;
using DocManager.Models;
using DocManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentManagerController : ControllerBase
    {
        #region Properties and Variables

        private readonly ILogger<DocumentManagerController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;

        #endregion

        #region Constructors
        public DocumentManagerController(ILogger<DocumentManagerController> logger,
            IUnitOfWork unitOfWork,
            IDocumentService documentService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _documentService = documentService;
        }

        #endregion

        #region Endpoints

        [HttpPost("uploadfile")]
        public async Task<IActionResult> CreateDocument(DocumentModel document)
        {
            if (await _documentService.SaveDocument(document))
                return Ok();

            return new JsonResult("Something went wrong with the upload") { StatusCode = 500 };
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetDocumentByName(string fileName)
        {
            var data = await _documentService.DownloadDocument(fileName);
            if (data == null)
                return NotFound();
            return new FileContentResult(data, "application/octet-stream") { FileDownloadName = fileName };
        }

        [HttpGet("listalluploaded")]
        public async Task<IActionResult> ListAllUploadedDocuments()
        {
            return Ok(await _documentService.GetDocuments());
        }

        [HttpDelete("deletealluploaded")]
        public async Task<IActionResult> DeleteAllUploadedDocuments()
        {
            return Ok(await _documentService.DeleteAllDocuments());
        }

        #endregion
    }
}
