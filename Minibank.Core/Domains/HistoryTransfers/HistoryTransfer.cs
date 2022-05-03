using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core.Domains.HistoryTransfers
{
    public class HistoryTransfer
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
