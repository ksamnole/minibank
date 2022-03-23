using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Data.HistoryTransfers
{
    public class HistoryTransferDbModel
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public Currency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
