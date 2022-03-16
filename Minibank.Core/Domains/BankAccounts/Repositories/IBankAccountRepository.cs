using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccount Get(string id);
        IEnumerable<BankAccount> GetAll();
        void Create(BankAccount bankAccount);
        void Delete(string id);
        void CloseAccount(string id);
    }
}
