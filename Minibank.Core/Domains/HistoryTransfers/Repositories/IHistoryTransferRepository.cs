using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.HistoryTransfers.Repositories
{
    public interface IHistoryTransferRepository
    {
        public IEnumerable<HistoryTransfer> GetAll();
        public void Create(HistoryTransfer historyTransfer);
    }
}
