using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Loan;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book mappings
            CreateMap<CreateBookDto, Book>();

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Stock,
                    opt => opt.MapFrom(src => src.Stock));

            // Loan mappings
            CreateMap<CreateLoanDto, Loan>();

            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.BookTitle,
                    opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))
                .ForMember(dest => dest.IsOverdue,
                    opt => opt.MapFrom(src => CalculateIsOverdue(src)));

            // Mappings inversos si es necesario
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<LoanDto, Loan>().ReverseMap();
        }

        private static bool CalculateIsOverdue(Loan loan)
        {
            if (loan.Status == "Returned" || !loan.ReturnDate.HasValue)
                return false;

            // Suponiendo préstamos de 14 días
            var dueDate = loan.LoanDate.AddDays(14);
            return DateTime.UtcNow > dueDate;
        }
    }
}
