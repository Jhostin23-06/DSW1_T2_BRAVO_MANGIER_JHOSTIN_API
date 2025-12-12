using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface ILoanRepository : IRepository<Loan>
    {
        // Métodos específicos para Loan
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetLoansByStudentAsync(string studentName);
        Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId);
        Task<IEnumerable<Loan>> GetOverdueLoansAsync(DateTime currentDate);
        Task<Loan?> GetActiveLoanByBookAndStudentAsync(int bookId, string studentName);
        Task<bool> HasActiveLoanAsync(int bookId, string studentName);
        Task<int> CountActiveLoansByStudentAsync(string studentName);
    }
}
