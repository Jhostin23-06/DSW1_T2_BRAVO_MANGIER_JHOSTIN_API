using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; } // Foreign Key
        public string StudentName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public virtual Book Book { get; set; } = new Book();
    }
}
