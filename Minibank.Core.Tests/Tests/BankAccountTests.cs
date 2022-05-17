using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
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
using Moq;
using Xunit;

namespace Minibank.Core.Tests.Tests
{
    public class BankAccountTests
    {
        private readonly Mock<IBankAccountRepository> _mockBankAccountRepository;
        private readonly Mock<IHistoryTransferRepository> _mockHistoryTransferRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IValidator<BankAccount> _bankAccountValidator;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountTests()
        {
            _mockBankAccountRepository = new Mock<IBankAccountRepository>();
            _mockHistoryTransferRepository = new Mock<IHistoryTransferRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _bankAccountValidator = new BankAccountValidator(_mockBankAccountRepository.Object);
            var currencyConverter = new CurrencyConverter(new Mock<IExchangeRates>().Object);
            _bankAccountService = new BankAccountService(
                _mockBankAccountRepository.Object,
                currencyConverter,
                _mockHistoryTransferRepository.Object,
                _mockUnitOfWork.Object,
                _bankAccountValidator
                );
        }

        [Fact]
        public async Task GetBankAccountById_SuccessPath_ReturnBankAccountModel()
        {
            _mockBankAccountRepository
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
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetAll(CancellationToken.None))
                .ReturnsAsync(fakeBankAccounts);
            
            var bankAccounts = await _bankAccountService.GetAll(CancellationToken.None);

            _mockBankAccountRepository.Verify(repository => repository.GetAll(CancellationToken.None));
            
            Assert.Equal(fakeBankAccounts, bankAccounts);
        }

        [Theory]
        [InlineData(100, "1", "1", 0)]
        [InlineData(100, "1", "2", 2)]
        public async Task CalculateCommission_SuccessPath(int amount, string firstId, string secondId, float expectedResult)
        {
            var firstBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var secondBankAccount = new BankAccount() { Id = "2", UserId = "2" };
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetById("1", CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _mockBankAccountRepository
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
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(firstBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(secondBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(secondBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.CalculateCommission(0, firstBankAccount.Id, firstBankAccount.Id, CancellationToken.None));
            
            Assert.Equal("Сумма не может быть меньше или равна 0", exception.Message);
        }
        
        [Fact]
        public async Task CalculateCommission_NotExistAccount_ShouldThrowException()
        {
            var firstBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var secondBankAccountId = "2";
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(firstBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(firstBankAccount);
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(secondBankAccountId, CancellationToken.None))
                .ThrowsAsync(new ObjectNotFoundException($"Банковский аккаунт с Id = {secondBankAccountId} не найден"));

            var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(
                () => _bankAccountService.CalculateCommission(10, firstBankAccount.Id, secondBankAccountId, CancellationToken.None));
            
            Assert.Equal($"Банковский аккаунт с Id = {secondBankAccountId} не найден", exception.Message);
        }
        
        [Fact]
        public async Task TransferMoney_WithZeroAmount_ShouldThrowException()
        {
            var fromBankAccount = new BankAccount() { Id = "1", UserId = "1" };
            var toBankAccount = new BankAccount() { Id = "2", UserId = "2" };
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(fromBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromBankAccount);
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(toBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(toBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.TransferMoney(0, fromBankAccount.Id, toBankAccount.Id, CancellationToken.None));
            
            Assert.Equal("Сумма перевода должна быть больше нуля", exception.Message);
        }
        
        [Fact]
        public async Task TransferMoney_WithZeroBalance_ShouldThrowException()
        {
            var amount = 100;
            var fromBankAccount = new BankAccount() { Id = "1", UserId = "1", Currency = Currency.RUB, Amount = 0};
            var toBankAccount = new BankAccount() { Id = "2", UserId = "2", Currency = Currency.RUB, Amount = 0};
            
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(fromBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromBankAccount);
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(toBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(toBankAccount);

            var exception = await Assert.ThrowsAsync<ValidationException>( 
                () => _bankAccountService.TransferMoney(amount, fromBankAccount.Id, toBankAccount.Id,CancellationToken.None));
            
            Assert.Equal("На счете недостаточно средств",exception.Message);
        }

        [Theory]
        // Different userId
        [InlineData(100, 102, 0, "1", "2", new[] {0d, 100})]
        [InlineData(10, 100, 0, "1", "2", new[] {89.8d, 10})]
        // Same userId
        [InlineData(100, 100, 0, "1", "1", new[] {0d, 100})]
        [InlineData(10, 10, 0, "1", "1", new[] {0d, 10})]
        public async Task TransferMoney_SuccessPathSameCurrency_CreateCalledOneTime(
            float amount, float amountFromBankAccount, float amountToBankAccount, string userIdFromBankAccount, string userIdToBankAccount, double[] expectedResult)
        {
            var currency = Currency.RUB;
            var fromBankAccount = new BankAccount() { Id = "1", UserId = userIdFromBankAccount, Currency = currency, Amount = amountFromBankAccount};
            var toBankAccount = new BankAccount() { Id = "2", UserId = userIdToBankAccount, Currency = currency, Amount = amountToBankAccount};

            _mockBankAccountRepository
                .Setup(repository => repository.GetById(fromBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(fromBankAccount);
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(toBankAccount.Id, CancellationToken.None))
                .ReturnsAsync(toBankAccount);

            await _bankAccountService.TransferMoney(amount, fromBankAccount.Id, toBankAccount.Id,CancellationToken.None);
            
            _mockBankAccountRepository.Verify(repository => repository.Update(fromBankAccount, CancellationToken.None));
            _mockBankAccountRepository.Verify(repository => repository.Update(toBankAccount, CancellationToken.None));
            _mockHistoryTransferRepository.Verify(repository => repository.Create(It.IsAny<HistoryTransfer>(), CancellationToken.None));
            _mockUnitOfWork.Verify(verify => verify.SaveChange());
            
            Assert.Equal(expectedResult[0], fromBankAccount.Amount);
            Assert.Equal(expectedResult[1],toBankAccount.Amount);
        }

        [Fact]
        public async Task AddBankAccount_SuccessPath_CreateCalledOneTime()
        {
            var bankAccount = new BankAccount() { Currency = Currency.RUB, Amount = 100, OpenDate = DateTime.Now, UserId = "1" };

            await _bankAccountService.Create(bankAccount, CancellationToken.None);
            
            _mockBankAccountRepository.Verify(repository => repository.Create(bankAccount, CancellationToken.None));
            _mockUnitOfWork.Verify(verify => verify.SaveChange());
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
            
            _mockBankAccountRepository.Verify(repository => repository.Delete("fakeId", CancellationToken.None));
            _mockUnitOfWork.Verify(verify => verify.SaveChange());
        }
        
        [Fact]
        public async Task UpdateBankAccount_SuccessPath_UpdateCalledOneTime()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 100, Currency = Currency.EUR, IsActive = true, UserId = "1"};
            
            await _bankAccountService.Update(bankAccount, CancellationToken.None);
            
            _mockBankAccountRepository.Verify(repository => repository.Update(bankAccount, CancellationToken.None));
            _mockUnitOfWork.Verify(verify => verify.SaveChange());
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
            _mockBankAccountRepository
                .Setup(repository => repository.GetById("fakeId", CancellationToken.None))
                .ReturnsAsync(bankAccount);
            
            await _bankAccountService.CloseAccount("fakeId", CancellationToken.None);
            
            _mockBankAccountRepository.Verify(repository => repository.Update(bankAccount, CancellationToken.None));
            
            Assert.False(bankAccount.IsActive);
        }
        
        [Fact]
        public async Task CloseBankAccount_WithNotZeroAmount_ShouldThrowException()
        {
            var bankAccount = new BankAccount() { Id = "Id", Amount = 1, Currency = Currency.EUR, IsActive = true, UserId = "Id"};
            _mockBankAccountRepository
                .Setup(repository => repository.GetById(bankAccount.Id, CancellationToken.None))
                .ReturnsAsync(bankAccount);
            
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _bankAccountService.CloseAccount(bankAccount.Id, CancellationToken.None));

            Assert.Equal("Сумма на счету аккаунта должна быть 0", exception.Message);
            
            Assert.True(bankAccount.IsActive);
        }
    }
}