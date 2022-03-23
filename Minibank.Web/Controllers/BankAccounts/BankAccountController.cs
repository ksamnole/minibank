using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Web.Controllers.BankAccounts.Dto;
using System.Collections.Generic;
using System.Linq;

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
        public BankAccountDto Get(string id)
        {
            var model = _bankAccountService.GetById(id);

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
        public IEnumerable<BankAccountDto> GetAll()
        {
            return _bankAccountService.GetAll().Select(it => new BankAccountDto() {
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
        public float CalculateCommission(float amount, string fromAccountId, string toAccountId)
        {
            return _bankAccountService.CalculateCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost("transfer")]
        public void TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            _bankAccountService.TransferMoney(amount, fromAccountId, toAccountId);
        }

        [HttpPut("deposit/{id}")]
        public void Deposit(string id, float amout)
        {
            var entity = _bankAccountService.GetById(id);

            _bankAccountService.Update(new BankAccount()
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
        public void Create(string userId, Currency currency)
        {
            _bankAccountService.Create(new BankAccount
            {
                UserId = userId,
                Currency = currency
            });
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _bankAccountService.Delete(id);
        }

        [HttpGet("close/{id}")]
        public void CloseAccount(string id)
        {
            _bankAccountService.CloseAccount(id);
        }
    }
}
