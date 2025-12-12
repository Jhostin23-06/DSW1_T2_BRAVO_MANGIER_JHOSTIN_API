using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces.Book;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            return await _unitOfWork.Book.ExistsAsync(id);
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            var isbnExists = await _unitOfWork.Book.ISBNExistsAsync(createBookDto.ISBN);
            if (isbnExists)
            {
                throw new DuplicateEntityException("Book", "ISBN", createBookDto.ISBN);
            }

            // REGLA: Stock no puede ser negativo
            if (createBookDto.Stock < 0)
            {
                throw new BusinessRuleException("InvalidStock", "El stock no puede ser negativo");
            }

            var book = _mapper.Map<Library.Domain.Entities.Book>(createBookDto);
            book.CreatedAt = DateTime.UtcNow;

            var createdBook = await _unitOfWork.Book.CreateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookDto>(createdBook);

        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _unitOfWork.Book.GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }

            // REGLA: No se puede eliminar libro con préstamos activos
            var hasActiveLoans = book.Loans?.Any(l => l.Status == "Active") ?? false;
            if (hasActiveLoans)
            {
                throw new BusinessRuleException("BookHasActiveLoans",
                    "No se puede eliminar un libro con préstamos activos");
            }

            var result = await _unitOfWork.Book.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _unitOfWork.Book.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> GetAvailableBooksAsync()
        {
            var books = await _unitOfWork.Book.GetAvailableBooksAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.Book.GetByIdAsync(id);
            return book != null ? _mapper.Map<BookDto>(book) : null;
        }

        public async Task<BookDto?> GetBookByISBNAsync(string isbn)
        {
            var book = await _unitOfWork.Book.GetByISBNAsync(isbn);
            return book != null ? _mapper.Map<BookDto>(book) : null;
        }

        public async Task<IEnumerable<BookDto>> SearchBooksAsync(string? title, string? author)
        {
            IEnumerable<Library.Domain.Entities.Book> books;

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
            {
                var booksByTitle = await _unitOfWork.Book.SearchByTitleAsync(title);
                var booksByAuthor = await _unitOfWork.Book.SearchByAuthorAsync(author);
                books = booksByTitle.Intersect(booksByAuthor);
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                books = await _unitOfWork.Book.SearchByTitleAsync(title);
            }
            else if (!string.IsNullOrWhiteSpace(author))
            {
                books = await _unitOfWork.Book.SearchByAuthorAsync(author);
            }
            else
            {
                books = await _unitOfWork.Book.GetAllAsync();
            }

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> UpdateBookAsync(int id, CreateBookDto updateBookDto)
        {
            var existingBook = await _unitOfWork.Book.GetByIdAsync(id);
            if (existingBook == null)
            {
                throw new NotFoundException("Book", id);
            }

            // REGLA: Validar unicidad de ISBN (excluyendo el libro actual)
            if (existingBook.ISBN != updateBookDto.ISBN)
            {
                var isbnExists = await _unitOfWork.Book.ISBNExistsAsync(updateBookDto.ISBN, id);
                if (isbnExists)
                {
                    throw new DuplicateEntityException("Book", "ISBN", updateBookDto.ISBN);
                }
            }

            // REGLA: Stock no puede ser negativo
            if (updateBookDto.Stock < 0)
            {
                throw new BusinessRuleException("InvalidStock", "El stock no puede ser negativo");
            }

            // REGLA: No se puede reducir stock por debajo de préstamos activos
            var activeLoansCount = existingBook.Loans?.Count(l => l.Status == "Active") ?? 0;
            if (updateBookDto.Stock < activeLoansCount)
            {
                throw new BusinessRuleException("InsufficientStock",
                    $"No se puede reducir el stock por debajo de {activeLoansCount} (préstamos activos existentes)");
            }

            // Actualizar propiedades
            existingBook.Title = updateBookDto.Title;
            existingBook.Author = updateBookDto.Author;
            existingBook.ISBN = updateBookDto.ISBN;
            existingBook.Stock = updateBookDto.Stock;

            var updatedBook = await _unitOfWork.Book.UpdateAsync(existingBook);
            await _unitOfWork.SaveChangesAsync();


            return _mapper.Map<BookDto>(updatedBook);
        }
    }
}
