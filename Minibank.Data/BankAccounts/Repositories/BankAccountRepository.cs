using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private static List<BankAccountDbModel> bankAccountStorage = new List<BankAccountDbModel>();

        public BankAccount Get(string id)
        {
            var entity = bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
                return null;

            return new BankAccount
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                UserId = entity.UserId,
                Amount = entity.Amount,
                Currency = entity.Currency,
                OpenDate = entity.OpenDate,
                CloseDate = entity.CloseDate
            };
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return bankAccountStorage.Select(it =>
            new BankAccount
            {
                Id = it.Id,
                IsActive = it.IsActive,
                UserId = it.UserId,
                Amount = it.Amount,
                Currency = it.Currency,
                OpenDate = it.OpenDate,
                CloseDate = it.CloseDate
            });
        }

        public void Create(BankAccount bankAccount)
        {
            var entity = new BankAccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                UserId = bankAccount.UserId,
                Amount = bankAccount.Amount,
                Currency = bankAccount.Currency,
                OpenDate = DateTime.Now
            };

            bankAccountStorage.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
                bankAccountStorage.Remove(entity);
        }

        public void CloseAccount(string id)
        {
            var entity = bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                entity.IsActive = false;
                entity.CloseDate = DateTime.Now;
            }
        }
    }
}
