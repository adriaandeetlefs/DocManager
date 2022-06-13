using DocManager.DAL.Entities;
using DocManager.DAL.Interfaces;
using DocManager.Models;
using DocManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Services.Services
{
    public class DocumentService : IDocumentService
    {
        #region Properties and Variables

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobService _azureBlobService;

        #endregion

        #region Constructors
        public DocumentService(IUnitOfWork unitOfWork,IAzureBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _azureBlobService = blobService;
        }

        #endregion

        #region Public Methods
        public async Task<byte[]> DownloadDocument(string FileName)
        {
            var documentExists = _unitOfWork.Documents.All().Result.Where(x => x.FileName == FileName).FirstOrDefault();

            if (documentExists == null)
                return null;

            //Get the blob
            return await _azureBlobService.GetBlob(FileName);
        }

        public Task<List<DocumentModel>> GetDocuments()
        {
            List<DocumentModel> documents = new List<DocumentModel>();

            //Query local database for the files uploaded
            var listOfLocal = _unitOfWork.Documents.All().Result.OrderByDescending(x => x.DateModified).ToList();

            foreach (Document document in listOfLocal)
            {
                documents.Add(new DocumentModel()
                {
                    DateModified = document.DateModified,
                    FileName = document.FileName,
                    FileSize = document.FileSize,
                    Id = document.Id
                });
            }

            return Task.FromResult(documents);
        }

        public async Task<bool> SaveDocument(DocumentModel document)
        {
            document.DateModified = DateTime.Now;

            Document doc = new Document();
            doc.DateModified = document.DateModified;
            doc.FileName = document.FileName;
            doc.FileSize = document.FileSize;

            await _unitOfWork.Documents.Add(doc);
            await _unitOfWork.CompleteAsync();

            //Add it to the Azure Blob storage
            await _azureBlobService.UploadContentBlobAsync(document.FileContent, document.FileName);

            return true;
        }

        public async Task<bool> DeleteAllDocuments()
        {
            var listOfLocal = _unitOfWork.Documents.All().Result.OrderByDescending(x => x.DateModified).ToList();

            foreach (Document d in listOfLocal)
            {
                await _azureBlobService.DeleteBlobAsync(d.FileName);
                await _unitOfWork.Documents.Delete(d.Id);
            }

            await _unitOfWork.CompleteAsync();

            return true;
        }

        #endregion
    }
}
