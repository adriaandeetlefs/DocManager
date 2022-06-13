using DocManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Services.Interfaces
{
    public interface IDocumentService
    {
        public Task<List<DocumentModel>> GetDocuments();
        public Task<bool> SaveDocument(DocumentModel document);
        public Task<byte[]> DownloadDocument(string FileName);
        public Task<bool> DeleteAllDocuments();
    }
}
