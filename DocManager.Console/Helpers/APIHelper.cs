using DocManager.Helpers.Extensions;
using DocManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.Client.Helpers
{
    public class APIHelper
    {
        #region Properties and Variables
        private string _apiEndpoint { get; set; }
        private string _apiUploadEndpoint { get; set; }
        private string _apiDownloadEndpoint { get; set; }
        private string _apiListEndpoint { get; set; }

        #endregion

        #region Constructors
        public APIHelper(string apiEndpoint)
        {
            _apiEndpoint = apiEndpoint;
            _apiUploadEndpoint = _apiEndpoint + "/DocumentManager/uploadfile";
            _apiDownloadEndpoint = _apiEndpoint + "/DocumentManager/";
            _apiListEndpoint = _apiEndpoint + "/DocumentManager/listalluploaded";
        }

        #endregion

        #region Public Methods

        public async Task<Tuple<bool,string>> UploadDocument(string filePath)
        {
            if (!File.Exists(filePath))
                return new Tuple<bool, string>(false, "File does not exist!");

            using (HttpClient client = new HttpClient())
            {
                using (var fileForm = new MultipartFormDataContent())
                {
                    DocumentModel document = new DocumentModel();
                    document.ContentType = filePath.GetContentType();
                    document.FileContent = File.ReadAllBytes(filePath);
                    document.FileName = Path.GetFileName(filePath);
                    document.FileSize = new FileInfo(filePath).Length;

                    var json = JsonConvert.SerializeObject(document);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = client.PostAsync(_apiUploadEndpoint, data);
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var modelString = response.Result;
                        return new Tuple<bool, string>(true, "File Uploaded");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "File upload was unsuccessful");
                    }
                }
            }
        }

        public async Task<Tuple<bool,List<DocumentModel>>> GetDocumentList()
        {
            using (HttpClient client = new HttpClient())
            {
                using (var fileForm = new MultipartFormDataContent())
                {
                    var response = client.GetAsync(_apiListEndpoint);
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var responseString = response.Result.Content.ReadAsStringAsync();
                        List<DocumentModel> documents = JsonConvert.DeserializeObject<List<DocumentModel>>(responseString.Result);

                        return new Tuple<bool, List<DocumentModel>>(true,documents);
                    }
                    else
                    {
                        return new Tuple<bool, List<DocumentModel>>(false, new List<DocumentModel>());
                    }
                }
            }
        }

        public async Task<Tuple<bool, string>> DownloadDocument(string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var fileForm = new MultipartFormDataContent())
                {
                    var response = client.GetAsync(string.Format(_apiDownloadEndpoint+"{0}",fileName));
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var result = await response.Result.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(fileName, result);

                        return new Tuple<bool, string>(true, fileName);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, response.Result.ReasonPhrase);
                    }
                }
            }
        }

        #endregion
    }
}
