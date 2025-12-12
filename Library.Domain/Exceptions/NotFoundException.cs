using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public NotFoundException(string entityName, object entityId)
          : base($"{entityName} with ID {entityId} not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
