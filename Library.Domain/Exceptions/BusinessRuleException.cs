using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Exceptions
{
    public class BusinessRuleException : DomainException
    {
        public string RuleName { get; set; }
        public BusinessRuleException(string ruleName, string message) : base(message)
        {
            RuleName = ruleName;
        }
    }
}
