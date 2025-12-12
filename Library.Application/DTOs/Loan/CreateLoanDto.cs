using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Loan
{
    public class CreateLoanDto
    {
        [Required(ErrorMessage = "El ID del libro es requerido")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "El nombre del estudiante es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string StudentName { get; set; } = string.Empty;
    }
}
