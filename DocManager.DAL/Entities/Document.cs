using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DAL.Entities
{
    [Table(name: "Document")]
    public class Document
    {
        public int Id { get; set; }        
        public string FileName { get; set; }
        public DateTime DateModified { get; set; }
        public double FileSize { get; set; }

    }
}
