using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public class UserFriendlyException : Exception
    {
        public float Value { get; }
        public UserFriendlyException(string message, float value) : base(message)
        {
            Value = value;
        }
    }
}
