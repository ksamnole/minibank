using Minibank.Core.Domains.BankAccounts.Services;

namespace Minibank.Core.Domains.HistoryTransfers
{
    public class HistoryTransfer
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public AllowedCurrency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
