using DocManager.DAL.Core.Context;
using DocManager.DAL.Interfaces;
using DocManager.DAL.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DAL.Entities
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Properties and Variables

        private readonly FileCoreDbContext _context;
        private readonly ILogger _logger;
        
        public IDocumentRepository Documents { get; private set; }

        #endregion

        #region Constructors
        public UnitOfWork(FileCoreDbContext context,
                    ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Documents = new DocumentRepository(_context, _logger);
        }

        #endregion

        #region Public Methods

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
