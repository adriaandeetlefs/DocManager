using DocManager.DAL.Core.Context;
using DocManager.DAL.Entities;
using DocManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DAL.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        #region Constructors
        public DocumentRepository(FileCoreDbContext context,
            ILogger logger) : base(context, logger)
        {

        }
        #endregion

        #region Public Methods
        public override async Task<IEnumerable<Document>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method error",typeof(DocumentRepository));
                return new List<Document>();
            }
        }

        public override async Task<bool> Upsert(Document entity)
        {
            try
            {
                var existingDocument = await dbSet.Where(x => x.FileName == entity.FileName).FirstOrDefaultAsync();

                if (existingDocument == null)
                {
                    return await Add(entity);
                }
                else
                {
                    existingDocument.FileSize = entity.FileSize;
                    existingDocument.DateModified = entity.DateModified;
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert method error", typeof(DocumentRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int Id)
        {
            try
            {
                var existingDocument = await dbSet.Where(x => x.Id == Id).FirstOrDefaultAsync();

                if (existingDocument != null)
                {
                    dbSet.Remove(existingDocument);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method error", typeof(DocumentRepository));
                return false;
            }
        }

        #endregion
    }
}
