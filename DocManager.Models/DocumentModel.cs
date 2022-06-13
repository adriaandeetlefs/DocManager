using System;

namespace DocManager.Models
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime DateModified { get; set; }
        public double FileSize { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
    }
}
