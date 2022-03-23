﻿using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.HistoryTransfers.Services;
using Minibank.Web.Controllers.HistoryTransfers.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Web.Controllers.HistoryTransfers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryTransferController : Controller
    {
        private readonly IHistoryTransferService _historyTransferService;

        public HistoryTransferController(IHistoryTransferService historyTransferService)
        {
            _historyTransferService = historyTransferService;
        }

        [HttpGet()]
        public IEnumerable<HistoryTransferDto> GetAll()
        {
            return _historyTransferService.GetAll()
                .Select(it => new HistoryTransferDto
                {
                    Id = it.Id,
                    Amount = it.Amount,
                    Currency = it.Currency,
                    FromAccountId = it.FromAccountId,
                    ToAccountId = it.ToAccountId
                });
        }
    }
}
