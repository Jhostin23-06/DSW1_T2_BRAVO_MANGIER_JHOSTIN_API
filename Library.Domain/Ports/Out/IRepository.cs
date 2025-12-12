using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface IRepository<T> where T : class
    {
        // READ
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        // CREATE
        Task<T> CreateAsync(T entity);

        // UPDATE  
        Task<T> UpdateAsync(T entity);

        // DELETE
        Task<bool> DeleteAsync(int id);

        // EXISTS
        Task<bool> ExistsAsync(int id);
    }
}
