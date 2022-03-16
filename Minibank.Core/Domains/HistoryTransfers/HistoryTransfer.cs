namespace Minibank.Core.Domains.HistoryTransfers
{
    public class HistoryTransfer
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
