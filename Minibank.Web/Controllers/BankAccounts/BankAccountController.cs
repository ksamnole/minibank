using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Web.Controllers.BankAccounts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            this.bankAccountService = bankAccountService;
        }

        [HttpGet("{id}")]
        public BankAccountDto Get(string id)
        {
            var model = bankAccountService.Get(id);

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
            return bankAccountService.GetAll().Select(it => new BankAccountDto() {
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
            return bankAccountService.CalculateCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost("create")]
        public void Create(string userId, string currency)
        {
            bankAccountService.Create(new BankAccount
            {
                UserId = userId,
                Currency = currency
            });
        }

        [HttpDelete("delete/{id}")]
        public void Delete(string id)
        {
            bankAccountService.Delete(id);
        }

        [HttpGet("close/{id}")]
        public void CloseAccount(string id)
        {
            bankAccountService.CloseAccount(id);
        }
    }
}
