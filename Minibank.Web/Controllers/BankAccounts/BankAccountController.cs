using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.HistoryTransfers.Repositories;
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
        private readonly IHistoryTransferRepository historyTransferRepository;

        public BankAccountController(IBankAccountService bankAccountService, IHistoryTransferRepository historyTransferRepository)
        {
            this.bankAccountService = bankAccountService;
            this.historyTransferRepository = historyTransferRepository;
        }

        [HttpGet("{id}")]
        public BankAccountDto Get(string id)
        {
            var model = bankAccountService.GetById(id);

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

        [HttpPost("transfer")]
        public void TransferMoney(float amount, string fromAccountId, string toAccountId)
        {
            bankAccountService.TransferMoney(amount, fromAccountId, toAccountId);
        }

        [HttpPut("deposit/{id}")]
        public void Deposit(string id, float amout)
        {
            var entity = bankAccountService.GetById(id);

            bankAccountService.Update(new BankAccount()
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

        [HttpPost("create")]
        public void Create(string userId, string currency)
        {
            bankAccountService.Create(new BankAccount
            {
                UserId = userId,
                Currency = currency
            });
        }

        [HttpPut("{id}")]
        public void Update(string id, BankAccountDto model)
        {
            bankAccountService.Update(new BankAccount()
            {
                Id = id,
                IsActive = model.IsActive,
                UserId = model.UserId,
                Amount = model.Amount,
                Currency = model.Currency,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate
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
