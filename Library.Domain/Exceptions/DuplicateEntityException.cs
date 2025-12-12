using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Exceptions
{
    public class DuplicateEntityException : DomainException
    {
        public string EntityName { get; }
        public string DuplicateField { get; }

        public DuplicateEntityException(string entityName, string field, string value)
          : base($"{entityName} with {field} '{value}' already exists.")
        {
            EntityName = entityName;
            DuplicateField = field;
        }
    }
}
