using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetById(string id);
        Task<IEnumerable<BankAccount>> GetAll();
        Task Create(BankAccount bankAccount);
        Task Update(BankAccount bankAccount);
        Task Delete(string id);
    }
}
