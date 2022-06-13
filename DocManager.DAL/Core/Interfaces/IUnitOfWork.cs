using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IDocumentRepository Documents { get; }
        Task CompleteAsync();

    }
}
