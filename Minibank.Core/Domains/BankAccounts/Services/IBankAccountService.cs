using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetById(string id);
        Task<IEnumerable<BankAccount>> GetAll();
        Task<float> CalculateCommission(float amount, string fromAccountId, string toAccountId);
        Task TransferMoney(float amount, string fromAccountId, string toAccountId);
        Task Create(BankAccount bankAccount);
        Task Update(BankAccount bankAccount);
        Task Delete(string id);
        Task CloseAccount(string id);
    }
}
