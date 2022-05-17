using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Web.Controllers.HistoryTransfers.Dto
{
    public class HistoryTransferDto
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
