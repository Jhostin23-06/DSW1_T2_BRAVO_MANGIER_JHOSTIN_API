using Library.Application.DTOs.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Loan
{
    public interface ILoanService
    {
        Task<LoanDto?> GetLoanByIdAsync(int id);
        Task<IEnumerable<LoanDto>> GetAllLoansAsync();
        Task<IEnumerable<LoanDto>> GetActiveLoansAsync();
        Task<IEnumerable<LoanDto>> GetLoansByStudentAsync(string studentName);
        Task<IEnumerable<LoanDto>> GetLoansByBookAsync(int bookId);
        Task<LoanDto> CreateLoanAsync(CreateLoanDto createLoanDto);
        Task<LoanDto> ReturnLoanAsync(int id);
        Task<bool> DeleteLoanAsync(int id);
        Task<bool> LoanExistsAsync(int id);
    }
}
