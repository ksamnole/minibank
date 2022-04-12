using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public interface IHistoryTransferService
    {
        public Task<IEnumerable<HistoryTransfer>> GetAll(CancellationToken cancellationToken);
        public Task Create(HistoryTransfer historyTransfer, CancellationToken cancellationToken);
    }
}
