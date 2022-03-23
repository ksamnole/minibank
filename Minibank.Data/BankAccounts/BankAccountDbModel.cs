using Minibank.Core.Domains.BankAccounts.Enums;
using System;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountDbModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public float Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}
