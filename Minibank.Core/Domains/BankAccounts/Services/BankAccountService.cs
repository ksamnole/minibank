using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AllowedCurrency
    {
        RUB,
        USD,
        EUR
    }

    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoryTransferRepository _historyTransferRepository;
        private readonly ICurrencyConverter _currencyConverter;

        public BankAccountService(IBankAccountRepository bankAccountRepository, ICurrencyConverter currencyConverter = null, IHistoryTransferRepository historyTransferRepository = null)
        {
            this._bankAccountRepository = bankAccountRepository;
            this._historyTransferRepository = historyTransferRepository;
            this._currencyConverter = currencyConverter;
        }

        public BankAccount GetById(string id)
        {
            return _bankAccountRepository.GetById(id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public float CalculateCommission(float amount, string fromAccountId, string toAccountId)
        {
            if (amount < 0)
                throw new ValidationException("Сумма не может быть меньше 0", amount);

            var fromAccount = _bankAccountRepository.GetById(fromAccountId);
            var toAccount = _bankAccountRepository.GetById(toAccountId);

            return CalculateCommission(amount, fromAccount, toAccount);
        }

        public void TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма перевода должна быть больше нуля", amount);

            var fromAccount = _bankAccountRepository.GetById(fromAccountId);
            var toAccount = _bankAccountRepository.GetById(toAccountId);

            var commission = CalculateCommission(amount, fromAccount, toAccount);

            if (fromAccount.Amount < amount + commission)
                throw new ValidationException("На счете недостаточно средств");

            float amountInToAccountCurrency;

            if (fromAccount.Currency != toAccount.Currency)
                amountInToAccountCurrency = _currencyConverter.Convert(amount, fromAccount.Currency, toAccount.Currency);
            else
                amountInToAccountCurrency = amount;


            fromAccount.Amount -= (amount + commission);
            _bankAccountRepository.Update(fromAccount);

            toAccount.Amount += amountInToAccountCurrency;
            _bankAccountRepository.Update(toAccount);

            _historyTransferRepository.Create(new HistoryTransfer() {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                Currency = fromAccount.Currency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }

        public void Create(BankAccount bankAccount)
        {
            if (!Enum.IsDefined(typeof(AllowedCurrency), bankAccount.Currency))
                throw new ValidationException("Запрещенная валюта", bankAccount.Currency);
            
            _bankAccountRepository.Create(bankAccount);
        }

        public void Update(BankAccount bankAccount)
        {
            _bankAccountRepository.Update(bankAccount);
        }

        public void Delete(string id)
        {
            _bankAccountRepository.Delete(id);
        }

        public void CloseAccount(string id)
        {
            var model = _bankAccountRepository.GetById(id);

            if (model.Amount != 0)
                throw new ValidationException("Сумма на счету аккаунта должна быть 0");

            model.IsActive = false;
            model.CloseDate = DateTime.Now;

            _bankAccountRepository.Update(model);
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
