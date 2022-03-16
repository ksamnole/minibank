using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }

    }
}
