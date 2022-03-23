using Minibank.Core.Domains.BankAccounts.Services;
using System;

namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public float Amount { get; set; }
        public AllowedCurrency Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

    }
}
