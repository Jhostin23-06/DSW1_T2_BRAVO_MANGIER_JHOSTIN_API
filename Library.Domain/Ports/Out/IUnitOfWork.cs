using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Ports.Out
{
    public interface IUnitOfWork : IDisposable
    {
        // Propiedades para acceder a los repositorios
        IBookRepository Book { get; }
        ILoanRepository Loan { get; }


        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
