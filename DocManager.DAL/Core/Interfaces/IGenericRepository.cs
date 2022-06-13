using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManager.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int Id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int Id);
        Task<bool> Upsert(T entity);
    }
}
