using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly DataContext _context;

        public BankAccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetById(string id)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == id);

            if (entity == null)
                throw new ObjectNotFoundException($"Банковский аккаунт с Id = {id} не найден");

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

        public async Task<IEnumerable<BankAccount>> GetAll()
        {
            return _context.BankAccounts.Select(it =>
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

        public async Task Create(BankAccount bankAccount)
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

            await _context.BankAccounts.AddAsync(entity);
        }

        public async Task Update(BankAccount bankAccount)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == bankAccount.Id);

            if (entity == null)
                throw new ObjectNotFoundException($"Банковский аккаунт с Id = {bankAccount.Id} не найден");

            entity.IsActive = bankAccount.IsActive;
            entity.UserId = bankAccount.UserId;
            entity.Amount = bankAccount.Amount;
            entity.Currency = bankAccount.Currency;
            entity.OpenDate = bankAccount.OpenDate;
            entity.CloseDate = bankAccount.CloseDate;
        }

        public async Task Delete(string id)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == id);

            if (entity == null)
                throw new ObjectNotFoundException($"Банковский аккаунт с Id = {id} не найден");

            _context.BankAccounts.Remove(entity);
        }
    }
}
