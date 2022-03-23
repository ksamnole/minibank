using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        BankAccount GetById(string id);
        IEnumerable<BankAccount> GetAll();
        float CalculateCommission(float amount, string fromAccountId, string toAccountId);
        void TransferMoney(float amount, string fromAccountId, string toAccountId);
        void Create(BankAccount bankAccount);
        void Update(BankAccount bankAccount);
        void Delete(string id);
        void CloseAccount(string id);
    }
}
