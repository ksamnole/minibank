using System;
using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

    }
}
