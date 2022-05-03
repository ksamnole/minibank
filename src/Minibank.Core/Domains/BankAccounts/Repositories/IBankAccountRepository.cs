using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetById(string id, CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken);
        Task Create(BankAccount bankAccount, CancellationToken cancellationToken);
        Task Update(BankAccount bankAccount, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}
