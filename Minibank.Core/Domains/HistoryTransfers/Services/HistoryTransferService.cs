using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public class HistoryTransferService : IHistoryTransferService
    {
        private readonly IHistoryTransferRepository historyTransferRepository;

        public HistoryTransferService(IHistoryTransferRepository historyTransferRepository)
        {
            this.historyTransferRepository = historyTransferRepository;
        }

        public void Create(HistoryTransfer historyTransfer)
        {
            historyTransferRepository.Create(historyTransfer);
        }

        public IEnumerable<HistoryTransfer> GetAll()
        {
            return historyTransferRepository.GetAll();
        }
    }
}
