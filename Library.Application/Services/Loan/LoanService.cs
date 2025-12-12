using AutoMapper;
using Library.Application.DTOs.Loan;
using Library.Application.Interfaces.Loan;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services.Loan
{
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoanDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _unitOfWork.Loan.GetByIdAsync(id);
            return loan != null ? _mapper.Map<LoanDto>(loan) : null;
        }

        public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
        {
            var loans = await _unitOfWork.Loan.GetAllAsync();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<IEnumerable<LoanDto>> GetActiveLoansAsync()
        {
            var loans = await _unitOfWork.Loan.GetActiveLoansAsync();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByStudentAsync(string studentName)
        {
            var loans = await _unitOfWork.Loan.GetLoansByStudentAsync(studentName);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByBookAsync(int bookId)
        {
            var loans = await _unitOfWork.Loan.GetLoansByBookIdAsync(bookId);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<LoanDto> CreateLoanAsync(CreateLoanDto createLoanDto)
        {
            // Validar que el libro existe
            var book = await _unitOfWork.Book.GetByIdAsync(createLoanDto.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book", createLoanDto.BookId);
            }

            // REGLA 1: No se puede prestar si stock es 0
            if (book.Stock <= 0)
            {
                throw new BusinessRuleException("NoStockAvailable",
                    $"No se puede prestar el libro '{book.Title}' porque no hay stock disponible");
            }

            // REGLA: Verificar si el estudiante ya tiene un préstamo activo del mismo libro
            var hasActiveLoan = await _unitOfWork.Loan.HasActiveLoanAsync(createLoanDto.BookId, createLoanDto.StudentName);
            if (hasActiveLoan)
            {
                throw new BusinessRuleException("DuplicateActiveLoan",
                    $"El estudiante '{createLoanDto.StudentName}' ya tiene un préstamo activo de este libro");
            }

            // REGLA: Límite de préstamos por estudiante (máximo 3)
            var activeLoansCount = await _unitOfWork.Loan.CountActiveLoansByStudentAsync(createLoanDto.StudentName);
            if (activeLoansCount >= 3)
            {
                throw new BusinessRuleException("StudentLoanLimit",
                    $"El estudiante '{createLoanDto.StudentName}' ha alcanzado el límite de 3 préstamos activos");
            }

            // Crear el préstamo
            var loan = new Library.Domain.Entities.Loan
            {
                BookId = createLoanDto.BookId,
                StudentName = createLoanDto.StudentName,
                LoanDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                Book = book
            };

            // REGLA 2: Disminuir stock del libro
            book.Stock--;

            var createdLoan = await _unitOfWork.Loan.CreateAsync(loan);
            await _unitOfWork.Book.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            var loanWithBook = await _unitOfWork.Loan.GetByIdAsync(createdLoan.Id);


            return _mapper.Map<LoanDto>(loanWithBook);
        }

        public async Task<LoanDto> ReturnLoanAsync(int id)
        {
            var loan = await _unitOfWork.Loan.GetByIdAsync(id);
            if (loan == null)
            {
                throw new NotFoundException("Loan", id);
            }

            // REGLA: No se puede devolver un préstamo ya devuelto
            if (loan.Status == "Returned")
            {
                throw new BusinessRuleException("LoanAlreadyReturned", "El préstamo ya ha sido devuelto");
            }

            // Obtener el libro asociado
            var book = await _unitOfWork.Book.GetByIdAsync(loan.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book", loan.BookId);
            }

            // Actualizar el préstamo
            loan.ReturnDate = DateTime.UtcNow;
            loan.Status = "Returned";

            // REGLA 3: Aumentar stock del libro
            book.Stock++;

            var updatedLoan = await _unitOfWork.Loan.UpdateAsync(loan);
            await _unitOfWork.Book.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            var loanWithBook = await _unitOfWork.Loan.GetByIdAsync(id);

            return _mapper.Map<LoanDto>(loanWithBook);
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await _unitOfWork.Loan.GetByIdAsync(id);
            if (loan == null)
            {
                return false;
            }

            // REGLA: No se puede eliminar un préstamo activo
            if (loan.Status == "Active")
            {
                throw new BusinessRuleException("CannotDeleteActiveLoan",
                    "No se puede eliminar un préstamo activo. Debe devolverlo primero.");
            }

            var result = await _unitOfWork.Loan.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return result;
        }

        public async Task<bool> LoanExistsAsync(int id)
        {
            return await _unitOfWork.Loan.ExistsAsync(id);
        }
    }
}
