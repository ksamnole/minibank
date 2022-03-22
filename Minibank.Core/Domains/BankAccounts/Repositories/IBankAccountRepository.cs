using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccount GetById(string id);
        IEnumerable<BankAccount> GetAll();
        void Create(BankAccount bankAccount);
        void Update(BankAccount bankAccount);
        void Delete(string id);
    }
}
