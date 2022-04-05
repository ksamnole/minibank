using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public interface IHistoryTransferService
    {
        public Task<IEnumerable<HistoryTransfer>> GetAll();
        public Task Create(HistoryTransfer historyTransfer);
    }
}
