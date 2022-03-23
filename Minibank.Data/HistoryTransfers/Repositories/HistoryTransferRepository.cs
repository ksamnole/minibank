using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.HistoryTransfers.Repositories
{
    public class HistoryTransferRepository : IHistoryTransferRepository
    {
        private static List<HistoryTransferDbModel> _historyTransfersStorage = new List<HistoryTransferDbModel>();

        public void Create(HistoryTransfer historyTransfer)
        {
            var entity = new HistoryTransferDbModel()
            {
                Id = historyTransfer.Id,
                Amount = historyTransfer.Amount,
                Currency = historyTransfer.Currency,
                FromAccountId = historyTransfer.FromAccountId,
                ToAccountId = historyTransfer.ToAccountId
            };

            _historyTransfersStorage.Add(entity);
        }

        public IEnumerable<HistoryTransfer> GetAll()
        {
            return _historyTransfersStorage.Select(it =>
            new HistoryTransfer
            {
                Id = it.Id,
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId,
                ToAccountId = it.ToAccountId
            });
        }
    }
}
