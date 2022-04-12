using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.HistoryTransfers.Services
{
    public class HistoryTransferService : IHistoryTransferService
    {
        private readonly IHistoryTransferRepository _historyTransferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HistoryTransferService(IHistoryTransferRepository historyTransferRepository, IUnitOfWork unitOfWork)
        {
            _historyTransferRepository = historyTransferRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(HistoryTransfer historyTransfer, CancellationToken cancellationToken)
        {
            await _historyTransferRepository.Create(historyTransfer, cancellationToken);
            await _unitOfWork.SaveChange();
        }

        public async Task<IEnumerable<HistoryTransfer>> GetAll(CancellationToken cancellationToken)
        {
            return await _historyTransferRepository.GetAll(cancellationToken);
        }
    }
}
