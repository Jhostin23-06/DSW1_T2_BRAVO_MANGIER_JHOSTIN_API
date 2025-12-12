using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface IBookRepository : IRepository<Book>
    {
        // Métodos específicos para Book
        Task<Book?> GetByISBNAsync(string isbn);
        Task<bool> ISBNExistsAsync(string isbn);
        Task<bool> ISBNExistsAsync(string isbn, int excludeId);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> SearchByTitleAsync(string title);
        Task<IEnumerable<Book>> SearchByAuthorAsync(string author);
    }
}
