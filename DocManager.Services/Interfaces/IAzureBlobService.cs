using Azure.Storage.Blobs.Models;
using DocManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Services.Interfaces
{
    public interface IAzureBlobService
    {
        // !Feedback: use camel casing not pascal casing for props
        Task<BlobDownloadResult> GetBlobAsync(string FileName);
        Task<byte[]> GetBlob(string FileName);
        Task<IEnumerable<string>> ListBlobsAsync();
        Task UploadFileBlobAsync(string filePath, string fileName);
        Task UploadContentBlobAsync(byte[] content, string fileName);
        Task DeleteBlobAsync(string blobName);
    }
}
