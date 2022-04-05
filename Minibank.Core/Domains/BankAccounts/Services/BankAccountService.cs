using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using Minibank.Core.Domains.BankAccounts.Enums;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoryTransferRepository _historyTransferRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IBankAccountRepository bankAccountRepository, ICurrencyConverter currencyConverter = null, IHistoryTransferRepository historyTransferRepository = null, IUnitOfWork unitOfWork = null)
        {
            _bankAccountRepository = bankAccountRepository;
            _historyTransferRepository = historyTransferRepository;
            _currencyConverter = currencyConverter;
            _unitOfWork = unitOfWork;
        }

        public async Task<BankAccount> GetById(string id)
        {
            return await _bankAccountRepository.GetById(id);
        }

        public async Task<IEnumerable<BankAccount>> GetAll()
        {
            return await _bankAccountRepository.GetAll();
        }

        public async Task<float> CalculateCommission(float amount, string fromAccountId, string toAccountId)
        {
            if (amount < 0)
                throw new ValidationException("Сумма не может быть меньше 0", amount);

            var fromAccount = await _bankAccountRepository.GetById(fromAccountId);
            var toAccount = await _bankAccountRepository.GetById(toAccountId);

            return CalculateCommission(amount, fromAccount, toAccount);
        }

        public async Task TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма перевода должна быть больше нуля", amount);

            var fromAccount = await _bankAccountRepository.GetById(fromAccountId);
            var toAccount = await _bankAccountRepository.GetById(toAccountId);

            var commission = CalculateCommission(amount, fromAccount, toAccount);

            if (fromAccount.Amount < amount + commission)
                throw new ValidationException("На счете недостаточно средств");

            float amountInToAccountCurrency;

            if (fromAccount.Currency != toAccount.Currency)
                amountInToAccountCurrency = _currencyConverter.Convert(amount, fromAccount.Currency, toAccount.Currency);
            else
                amountInToAccountCurrency = amount;


            fromAccount.Amount -= (amount + commission);
            await _bankAccountRepository.Update(fromAccount);

            toAccount.Amount += amountInToAccountCurrency;
            await _bankAccountRepository.Update(toAccount);

            await _historyTransferRepository.Create(new HistoryTransfer() {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                Currency = fromAccount.Currency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });

            await _unitOfWork.SaveChange();
        }

        public async Task Create(BankAccount bankAccount)
        {
            if (!Enum.IsDefined(typeof(Currency), bankAccount.Currency))
                throw new ValidationException("Запрещенная валюта", bankAccount.Currency);
            
            await _bankAccountRepository.Create(bankAccount);

            await _unitOfWork.SaveChange();
        }

        public async Task Update(BankAccount bankAccount)
        {
            await _bankAccountRepository.Update(bankAccount);

            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id)
        {
            await _bankAccountRepository.Delete(id);

            await _unitOfWork.SaveChange();
        }

        public async Task CloseAccount(string id)
        {
            var model = await _bankAccountRepository.GetById(id);

            if (model.Amount != 0)
                throw new ValidationException("Сумма на счету аккаунта должна быть 0");

            model.IsActive = false;
            model.CloseDate = DateTime.Now;

            await _bankAccountRepository.Update(model);

            await _unitOfWork.SaveChange();
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
