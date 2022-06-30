using DocManager.Models;
using DocManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using DocManager.Helpers.Extensions;

namespace DocManager.Services.Services
{
    public class AzureBlobService: IAzureBlobService
    {
        #region Properties and Variables
        private readonly BlobServiceClient _blobServiceClient;
        //Should probably inject through constructor :)
        private string _containerName = "adriaandeetlefs";

        #endregion

        #region Constructors
        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        #endregion

        #region Public Methods
          // !Feedback: no awaits code will run synchronously
        public async Task<BlobDownloadResult> GetBlobAsync(string FileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (!containerClient.Exists())
                containerClient.Create();

            var blobClient = containerClient.GetBlobClient(FileName);

            BlobDownloadResult dl = blobClient.DownloadContent();

            return dl;
        }
        public async Task<byte[]> GetBlob(string FileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (!containerClient.Exists())
                containerClient.Create();

            var blobClient = containerClient.GetBlobClient(FileName);
            // !Feedback: simplify using
            await using (MemoryStream memoryStream = new MemoryStream())
            {
                blobClient.DownloadTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }

            return items;
        }

        public async Task UploadFileBlobAsync(string filePath, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });
        }

        public async Task UploadContentBlobAsync(byte[] content, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var bytes = content;
            await using var memoryStream = new MemoryStream(bytes);

            await blobClient.UploadAsync(memoryStream);
        }

        public async Task DeleteBlobAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }


        #endregion
    }
}
