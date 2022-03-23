using Minibank.Core.Domains.BankAccounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.HistoryTransfers
{
    public class HistoryTransferDbModel
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public AllowedCurrency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
