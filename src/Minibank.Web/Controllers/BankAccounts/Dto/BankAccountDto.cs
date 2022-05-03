using Minibank.Core.Domains.BankAccounts.Enums;
using System;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountDto
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
