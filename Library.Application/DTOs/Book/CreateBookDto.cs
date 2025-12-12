using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs.Book
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es requerido")]
        [StringLength(150, ErrorMessage = "El autor no puede exceder 150 caracteres")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es requerido")]
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres")]
        public string ISBN { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }
}
