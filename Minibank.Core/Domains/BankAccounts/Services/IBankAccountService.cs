using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetById(string id, CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken);
        Task<float> CalculateCommission(float amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken);
        Task TransferMoney(float amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken);
        Task Create(BankAccount bankAccount, CancellationToken cancellationToken);
        Task Update(BankAccount bankAccount, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
        Task CloseAccount(string id, CancellationToken cancellationToken);
    }
}
