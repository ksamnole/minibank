using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IUserRepository userRepository;
        private readonly List<string> allowedCurrency;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository = null)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.userRepository = userRepository;
            allowedCurrency = new List<string>() { "RUB", "USD", "EUR" };
        }

        public BankAccount Get(string id)
        {
            return bankAccountRepository.Get(id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return bankAccountRepository.GetAll();
        }

        public float CalculateCommission(float amount, string fromAccountId, string toAccountId)
        {
            if (amount < 0)
                throw new ValidationException("Сумма не может быть меньше 0", amount);

            var fromAccount = bankAccountRepository.Get(fromAccountId);
            if (fromAccount == null)
                throw new ValidationException("Банковский аккаунт с таким Id не существует", fromAccountId);

            var toAccount = bankAccountRepository.Get(toAccountId);
            if (toAccount == null)
                throw new ValidationException("Банковский аккаунт с таким Id не существует", toAccountId);

            // Transfer between same users
            if (fromAccount.UserId == toAccount.UserId)
                return 0;

            var commission = Math.Round(amount * 0.02, 2);

            return (float)commission;

        }

        public void Create(BankAccount bankAccount)
        {
            // Validation
            if (userRepository.Get(bankAccount.UserId) == null)
                throw new ValidationException("Пользователя с такие Id не существует", bankAccount.UserId);
            if (!allowedCurrency.Contains(bankAccount.Currency))
                throw new ValidationException("Запрещенная валюта", bankAccount.Currency);
            
            bankAccountRepository.Create(bankAccount);
        }

        public void Delete(string id)
        {
            bankAccountRepository.Delete(id);
        }

        public void CloseAccount(string id)
        {
            var entity = bankAccountRepository.Get(id);

            if (entity.Amount != 0)
                throw new ValidationException("Сумма на счету аккаунта должна быть 0");

            bankAccountRepository.CloseAccount(id);
        }
    }
}
