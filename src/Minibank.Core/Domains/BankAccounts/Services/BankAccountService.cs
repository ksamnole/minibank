using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using Minibank.Core.Domains.BankAccounts.Enums;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IHistoryTransferRepository _historyTransferRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IValidator<BankAccount> _bankAccountValidator;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IBankAccountRepository bankAccountRepository, ICurrencyConverter currencyConverter, IHistoryTransferRepository historyTransferRepository, IUnitOfWork unitOfWork, IValidator<BankAccount> bankAccountValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _historyTransferRepository = historyTransferRepository;
            _currencyConverter = currencyConverter;
            _bankAccountValidator = bankAccountValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task<BankAccount> GetById(string id, CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.GetById(id, cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetAll(CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.GetAll(cancellationToken);
        }

        public async Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма не может быть меньше или равна 0", amount);

            var fromAccount = await _bankAccountRepository.GetById(fromAccountId, cancellationToken);
            var toAccount = await _bankAccountRepository.GetById(toAccountId, cancellationToken);

            return CalculateCommission(amount, fromAccount, toAccount);
        }

        public async Task TransferMoney(double amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма перевода должна быть больше нуля", amount);

            var fromAccount = await _bankAccountRepository.GetById(fromAccountId, cancellationToken);
            var toAccount = await _bankAccountRepository.GetById(toAccountId, cancellationToken);

            var commission = CalculateCommission(amount, fromAccount, toAccount);

            if (fromAccount.Amount < amount + commission)
                throw new ValidationException("На счете недостаточно средств");

            var amountInToAccountCurrency = fromAccount.Currency != toAccount.Currency
                ? _currencyConverter.Convert(amount, fromAccount.Currency, toAccount.Currency)
                : amount;

            fromAccount.Amount -= (amount + commission);
            await _bankAccountRepository.Update(fromAccount, cancellationToken);

            toAccount.Amount += amountInToAccountCurrency;
            await _bankAccountRepository.Update(toAccount, cancellationToken);

            await _historyTransferRepository.Create(new HistoryTransfer() {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                Currency = fromAccount.Currency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            }, cancellationToken);

            await _unitOfWork.SaveChange();
        }

        public async Task Create(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            await _bankAccountValidator.ValidateAndThrowAsync(bankAccount, cancellationToken);
            
            await _bankAccountRepository.Create(bankAccount, cancellationToken);

            await _unitOfWork.SaveChange();
        }

        public async Task Update(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            await _bankAccountValidator.ValidateAndThrowAsync(bankAccount, cancellationToken);
            
            await _bankAccountRepository.Update(bankAccount, cancellationToken);

            await _unitOfWork.SaveChange();
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _bankAccountRepository.Delete(id, cancellationToken);

            await _unitOfWork.SaveChange();
        }

        public async Task CloseAccount(string id, CancellationToken cancellationToken)
        {
            var model = await _bankAccountRepository.GetById(id, cancellationToken);

            if (model.Amount != 0)
                throw new ValidationException("Сумма на счету аккаунта должна быть 0");

            model.IsActive = false;
            model.CloseDate = DateTime.Now;

            await _bankAccountRepository.Update(model, cancellationToken);

            await _unitOfWork.SaveChange();
        }

        private static double CalculateCommission(double amount, BankAccount fromAccount, BankAccount toAccount)
        {
            // Transfer between same users
            if (fromAccount.UserId == toAccount.UserId)
                return 0;

            return Math.Round(amount * 0.02, 2);
        }
    }
}
