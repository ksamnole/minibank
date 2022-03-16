using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IHistoryTransferRepository historyTransferRepository;
        private readonly IUserRepository userRepository;
        private readonly ICurrencyConverter currencyConverter;
        private readonly List<string> allowedCurrency;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository = null, ICurrencyConverter currencyConverter = null, IHistoryTransferRepository historyTransferRepository = null)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.historyTransferRepository = historyTransferRepository;
            this.userRepository = userRepository;
            this.currencyConverter = currencyConverter;
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

            return CalculateCommission(amount, fromAccount, toAccount);
        }

        public void TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма перевода должна быть больше нуля", amount);

            var fromAccount = bankAccountRepository.Get(fromAccountId);
            if (fromAccount == null)
                throw new ValidationException("Банковский аккаунт с таким Id не существует", fromAccountId);

            var toAccount = bankAccountRepository.Get(toAccountId);
            if (toAccount == null)
                throw new ValidationException("Банковский аккаунт с таким Id не существует", toAccountId);

            var commission = CalculateCommission(amount, fromAccount, toAccount);

            if (fromAccount.Amount < amount + commission)
                throw new ValidationException("На счете недостаточно средств");

            float amountInToAccountCurrency;

            if (fromAccount.Currency != toAccount.Currency)
                amountInToAccountCurrency = currencyConverter.Convert(amount, fromAccount.Currency, toAccount.Currency);
            else
                amountInToAccountCurrency = amount;


            fromAccount.Amount -= (amount + commission);
            bankAccountRepository.Update(fromAccount);

            toAccount.Amount += amountInToAccountCurrency;
            bankAccountRepository.Update(toAccount);

            historyTransferRepository.Create(new HistoryTransfer() {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                Currency = fromAccount.Currency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
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

        public void Update(BankAccount bankAccount)
        {
            bankAccountRepository.Update(bankAccount);
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

        private float CalculateCommission(float amount, BankAccount fromAccount, BankAccount toAccount)
        {
            // Transfer between same users
            if (fromAccount.UserId == toAccount.UserId)
                return 0;

            var commission = Math.Round(amount * 0.02, 2);

            return (float)commission;
        }
    }
}
