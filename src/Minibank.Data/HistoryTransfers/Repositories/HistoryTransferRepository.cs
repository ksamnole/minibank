using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Data.HistoryTransfers.Repositories
{
    public class HistoryTransferRepository : IHistoryTransferRepository
    {
        private readonly DataContext _context;

        public HistoryTransferRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Create(HistoryTransfer historyTransfer, CancellationToken cancellationToken)
        {
            var entity = new HistoryTransferDbModel()
            {
                Id = historyTransfer.Id,
                Amount = historyTransfer.Amount,
                Currency = historyTransfer.Currency,
                FromAccountId = historyTransfer.FromAccountId,
                ToAccountId = historyTransfer.ToAccountId
            };

            await _context.Transfers.AddAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<HistoryTransfer>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Transfers.Select(it =>
            new HistoryTransfer
            {
                Id = it.Id,
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId,
                ToAccountId = it.ToAccountId
            }).ToListAsync(cancellationToken);
        }
    }
}
