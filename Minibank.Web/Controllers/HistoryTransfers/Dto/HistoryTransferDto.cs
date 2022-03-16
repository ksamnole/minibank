using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.HistoryTransfers.Dto
{
    public class HistoryTransferDto
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
