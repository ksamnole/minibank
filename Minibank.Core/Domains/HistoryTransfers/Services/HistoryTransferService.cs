using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public class HistoryTransferService : IHistoryTransferService
    {
        private readonly IHistoryTransferRepository _historyTransferRepository;

        public HistoryTransferService(IHistoryTransferRepository historyTransferRepository)
        {
            _historyTransferRepository = historyTransferRepository;
        }

        public void Create(HistoryTransfer historyTransfer)
        {
            _historyTransferRepository.Create(historyTransfer);
        }

        public IEnumerable<HistoryTransfer> GetAll()
        {
            return _historyTransferRepository.GetAll();
        }
    }
}
