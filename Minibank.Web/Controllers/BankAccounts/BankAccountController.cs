using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Web.Controllers.BankAccounts.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet("{id}")]
        public async Task<BankAccountDto> Get(string id)
        {
            var model = await _bankAccountService.GetById(id);

            return new BankAccountDto()
            {
                Id = model.Id,
                IsActive = model.IsActive,
                UserId = model.UserId,
                Amount = model.Amount,
                Currency = model.Currency,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate
            };
        }

        [HttpGet]
        public async Task<IEnumerable<BankAccountDto>> GetAll()
        {
            var bankAccounts = await _bankAccountService.GetAll();

            return bankAccounts.Select(it => new BankAccountDto() {
                Id = it.Id,
                IsActive = it.IsActive,
                UserId = it.UserId,
                Amount = it.Amount,
                Currency = it.Currency,
                OpenDate = it.OpenDate,
                CloseDate = it.CloseDate
            });
        }

        [HttpGet("commission")]
        public async Task<float> CalculateCommission(float amount, string fromAccountId, string toAccountId)
        {
            return await _bankAccountService.CalculateCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost("transfer")]
        public async Task TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            await _bankAccountService.TransferMoney(amount, fromAccountId, toAccountId);
        }

        [HttpPut("deposit/{id}")]
        public async Task Deposit(string id, float amout)
        {
            var entity = await _bankAccountService.GetById(id);

            await _bankAccountService.Update(new BankAccount()
            {
                Id = id,
                IsActive = entity.IsActive,
                UserId = entity.UserId,
                Amount = entity.Amount + amout,
                Currency = entity.Currency,
                OpenDate = entity.OpenDate,
                CloseDate = entity.CloseDate
            });
        }

        [HttpPost]
        public async Task Create(string userId, Currency currency)
        {
            await _bankAccountService.Create(new BankAccount
            {
                UserId = userId,
                Currency = currency
            });
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _bankAccountService.Delete(id);
        }

        [HttpGet("close/{id}")]
        public async Task CloseAccount(string id)
        {
            await _bankAccountService.CloseAccount(id);
        }
    }
}
