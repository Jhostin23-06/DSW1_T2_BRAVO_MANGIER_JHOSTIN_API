using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Library.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _dbSet
                .Where(b => b.Stock > 0)
                .ToListAsync();
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> ISBNExistsAsync(string isbn)
        {
            return await _dbSet.AnyAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> ISBNExistsAsync(string isbn, int excludeId)
        {
            return await _dbSet.AnyAsync(b => b.ISBN == isbn && b.Id != excludeId);
        }

        public async Task<IEnumerable<Book>> SearchByAuthorAsync(string author)
        {
            return await _dbSet
                .Where(b => b.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
        {
            return await _dbSet
                .Where(b => b.Title.Contains(title))
                .ToListAsync();
        }
    }
}
