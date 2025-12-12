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
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        public LoanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Loan?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(l => l.Book)  // ❌ AGREGAR AQUÍ
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public override async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _dbSet
                .Include(l => l.Book)  // ❌ AGREGAR AQUÍ
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public override async Task<Loan> CreateAsync(Loan entity)
        {
            await _dbSet.AddAsync(entity);

            // Cargar explícitamente el Book después de crear
            if (entity.BookId > 0)
            {
                // Esto fuerza la carga del Book
                await _context.Entry(entity)
                    .Reference(l => l.Book)
                    .LoadAsync();
            }

            return entity;
        }

        public override async Task<Loan> UpdateAsync(Loan entity)
        {
            _dbSet.Update(entity);

            // Cargar explícitamente el Book después de actualizar
            if (entity.BookId > 0 && entity.Book == null)
            {
                await _context.Entry(entity)
                    .Reference(l => l.Book)
                    .LoadAsync();
            }

            return entity;
        }

        public async Task<int> CountActiveLoansByStudentAsync(string studentName)
        {
            return await _dbSet
                .CountAsync(l =>
                    l.StudentName == studentName &&
                    l.Status == "Active");
        }

        public async Task<Loan?> GetActiveLoanByBookAndStudentAsync(int bookId, string studentName)
        {
            return await _dbSet
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l =>
                    l.BookId == bookId &&
                    l.StudentName == studentName &&
                    l.Status == "Active");
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _dbSet
                .Include(l => l.Book)
                .Where(l => l.Status == "Active")
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Include(l => l.Book)
                .Where(l => l.BookId == bookId)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByStudentAsync(string studentName)
        {
            return await _dbSet
                .Include(l => l.Book)
                .Where(l => l.StudentName == studentName)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync(DateTime currentDate)
        {
            var overdueDate = currentDate.AddDays(-14); // Préstamos de 14 días
            return await _dbSet
                .Include(l => l.Book)
                .Where(l => l.Status == "Active" && l.LoanDate < overdueDate)
                .ToListAsync();
        }

        public async Task<bool> HasActiveLoanAsync(int bookId, string studentName)
        {
            return await _dbSet
                .AnyAsync(l =>
                    l.BookId == bookId &&
                    l.StudentName == studentName &&
                    l.Status == "Active");
        }
    }
}
