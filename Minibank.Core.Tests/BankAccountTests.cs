using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.BankAccounts.Validators;
using Minibank.Core.Domains.HistoryTransfers;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
using Minibank.Data;
using Moq;
using Xunit;

namespace Minibank.Core.Tests
{
    public class BankAccountTests
    {
        private readonly Mock<IBankAccountRepository> _fakeBankAccountRepository;
        private readonly Mock<IHistoryTransferRepository> _fakeHistoryTransferRepository;
        private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
        private readonly IValidator<BankAccount> _bankAccountValidator;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountTests()
        {
            _fakeBankAccountRepository = new Mock<IBankAccountRepository>();
            _fakeHistoryTransferRepository = new Mock<IHistoryTransferRepository>();
            _fakeUnitOfWork = new Mock<IUnitOfWork>();
            _bankAccountValidator = new BankAccountValidator(_fakeBankAccountRepository.Object);
            var currencyConverter = new CurrencyConverter(new Mock<IExchangeRates>().Object);
            _bankAccountService = new BankAccountService(
                _fakeBankAccountRepository.Object,
                currencyConverter,
                _fakeHistoryTransferRepository.Object,
                _fakeUnitOfWork.Object,
                _bankAccountValidator
                );
        }

        [Fact]
        public async Task GetBankAccountById_SuccessPath_ReturnBankAccountModel()
        {
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new BankAccount() {Id = "fakeId"});

            var bankAccount = await _bankAccountService.GetById("fakeId", CancellationToken.None);
            
            Assert.Equal(typeof(BankAccount), bankAccount.GetType());
        }
        
        [Fact]
        public async Task GetAllBankAccounts_SuccessPath_ReturnListBankAccounts()
        {
            var fakeBankAccounts = new List<BankAccount>()
            {
                new BankAccount() { Id = "1" },
                new BankAccount() { Id = "2" },
            };
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetAll(CancellationToken.None))
                .ReturnsAsync(fakeBankAccounts);
            
            var bankAccounts = await _bankAccountService.GetAll(CancellationToken.None);

            _fakeBankAccountRepository.Verify(repository => repository.GetAll(CancellationToken.None));
            
            Assert.Equal(fakeBankAccounts, bankAccounts);
        }

        [Theory]
        [InlineData(100, "1", "1", 0)]
        [InlineData(100, "1", "2", 2)]
        public async Task CalculateCommission_SuccessPath(int amount, string firstId, string secondId, float expectedResult)
        {
            var firstBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var secondBankAccount = new BankAccount() { Id = "2", UserId = "2" };
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("2", CancellationToken.None))
                .ReturnsAsync(secondBankAccount);
            
            var result = await _bankAccountService.CalculateCommission(amount, firstId, secondId, CancellationToken.None);
            
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public async Task CalculateCommission_WithZeroAmount_ShouldThrowException()
        {
            var firstBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var secondBankAccount = new BankAccount() { Id = "2", UserId = "2" };
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("2", CancellationToken.None))
                .ReturnsAsync(secondBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.CalculateCommission(0, "1", "1", CancellationToken.None));
            
            Assert.Equal("Сумма не может быть меньше или равна 0", exception.Message);
        }
        
        [Fact]
        public async Task TransferMoney_WithZeroAmount_ShouldThrowException()
        {
            var firstBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var secondBankAccount = new BankAccount() { Id = "2", UserId = "2" };
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("2", CancellationToken.None))
                .ReturnsAsync(secondBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.TransferMoney(0, "1", "1", CancellationToken.None));
            
            Assert.Equal("Сумма перевода должна быть больше нуля", exception.Message);
        }
        
        [Fact]
        public async Task TransferMoney_WithZeroBalance_ShouldThrowException()
        {
            var amount = 100;
            var fromBankAccount = new BankAccount() { Id = "1", UserId = "1", Currency = Currency.RUB, Amount = 0};
            var toBankAccount = new BankAccount() { Id = "2", UserId = "2", Currency = Currency.RUB, Amount = 0};
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(fromBankAccount);
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("2", CancellationToken.None))
                .ReturnsAsync(toBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>( 
                () => _bankAccountService.TransferMoney(amount, fromBankAccount.Id, toBankAccount.Id,CancellationToken.None));
            
            Assert.Equal("На счете недостаточно средств",exception.Message);
        }
        
        [Fact]
        public async Task TransferMoney_SuccessPath_CreateCalledOneTime()
        {
            var amount = 100;
            var fromBankAccount = new BankAccount() { Id = "1", UserId = "1", Currency = Currency.RUB, Amount = 1000};
            var toBankAccount = new BankAccount() { Id = "2", UserId = "2", Currency = Currency.RUB, Amount = 0};
            
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(fromBankAccount);
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("2", CancellationToken.None))
                .ReturnsAsync(toBankAccount);

            await _bankAccountService.TransferMoney(amount, fromBankAccount.Id, toBankAccount.Id,CancellationToken.None);
            
            _fakeHistoryTransferRepository.Verify(repository => repository.Create(It.IsAny<HistoryTransfer>(), CancellationToken.None));
        }

        [Fact]
        public async Task AddBankAccount_SuccessPath_CreateCalledOneTime()
        {
            var bankAccount = new BankAccount() { Currency = Currency.RUB, Amount = 100, OpenDate = DateTime.Now, UserId = "1" };

            await _bankAccountService.Create(bankAccount, CancellationToken.None);
            
            _fakeBankAccountRepository.Verify(repository => repository.Create(bankAccount, CancellationToken.None));
        }
        
        [Fact]
        public async Task AddBankAccount_WithEmptyUserId_ShouldThrowException()
        {
            var bankAccount = new BankAccount() { Currency = Currency.RUB, Amount = 100, OpenDate = DateTime.Now, UserId = null };

            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _bankAccountService.Create(bankAccount, CancellationToken.None));
            var error = exception.Errors.First();
            
            Assert.Equal("UserId", error.PropertyName);
            Assert.Equal("Не должен быть пустым", error.ErrorMessage);
        }
        
        [Fact]
        public async Task DeleteBankAccount_SuccessPath_DeleteCalledOneTime()
        {
            await _bankAccountService.Delete("fakeId", CancellationToken.None);
            
            _fakeBankAccountRepository.Verify(repository => repository.Delete("fakeId", CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateBankAccount_SuccessPath_UpdateCalledOneTime()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 100, Currency = Currency.EUR, IsActive = true, UserId = "1"};
            
            await _bankAccountService.Update(bankAccount, CancellationToken.None);
            
            _fakeBankAccountRepository.Verify(repository => repository.Update(bankAccount, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateBankAccount_WithEmptyUserId_ShouldThrowException()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 100, Currency = Currency.EUR, IsActive = true, UserId = ""};
            
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _bankAccountService.Update(bankAccount, CancellationToken.None));
            var error = exception.Errors.First();
            
            Assert.Equal("UserId", error.PropertyName);
            Assert.Equal("Не должен быть пустым", error.ErrorMessage);
        }

        [Fact]
        public async Task CloseBankAccount_SuccessPath_UpdateCalledOneTime()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 0, Currency = Currency.EUR, IsActive = true, UserId = "Id"};
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById("fakeId", CancellationToken.None))
                .ReturnsAsync(bankAccount);
            
            await _bankAccountService.CloseAccount("fakeId", CancellationToken.None);
            
            _fakeBankAccountRepository.Verify(repository => repository.Update(bankAccount, CancellationToken.None));
        }
        
        [Fact]
        public async Task CloseBankAccount_WithNotZeroAmount_ShouldThrowException()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 1, Currency = Currency.EUR, IsActive = true, UserId = "Id"};
            _fakeBankAccountRepository
                .Setup(repository => repository.GetById(bankAccount.Id, CancellationToken.None))
                .ReturnsAsync(bankAccount);
            
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.CloseAccount(bankAccount.Id, CancellationToken.None));

            Assert.Equal("Сумма на счету аккаунта должна быть 0", exception.Message);
        }
    }
}