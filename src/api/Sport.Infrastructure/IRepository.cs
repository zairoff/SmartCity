using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Infrastructure.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<int> SaveChangesAsync();
        IQueryable<T> Get();
        IQueryable<T> GetAll();
    }
}
