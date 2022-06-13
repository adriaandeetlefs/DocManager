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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Properties and Variables

        protected FileCoreDbContext _context;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        #endregion

        #region Constructors
        public GenericRepository(FileCoreDbContext context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbSet = context.Set<T>();
        }

        #endregion

        #region Public Methods
        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public virtual Task<bool> Delete(int Id)
        {
            //Did not need this for the assessment
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(int Id)
        {
            return await dbSet.FindAsync(Id);
        }

        public virtual Task<bool> Upsert(T entity)
        {
            //Did not need this for the assessment
            throw new NotImplementedException();
        }

        #endregion
    }
}
