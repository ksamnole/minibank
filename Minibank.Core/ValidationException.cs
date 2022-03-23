using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public class ValidationException : Exception
    {
        public object Value { get; }

        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string message, object value) : base(message)
        {
            Value = value;
        }
    }
}
