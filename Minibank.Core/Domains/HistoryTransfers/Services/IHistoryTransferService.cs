using System.Collections;
using System.Collections.Generic;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public interface IHistoryTransferService
    {
        public IEnumerable<HistoryTransfer> GetAll();
        public void Create(HistoryTransfer historyTransfer);
    }
}
