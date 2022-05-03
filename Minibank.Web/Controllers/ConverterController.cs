using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Core.Domains.BankAccounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _converter;

        public ConverterController(ICurrencyConverter converter)
        {
            this._converter = converter;
        }

        [HttpGet("convert")]
        public double Convert(int amount, Currency fromCurrency, Currency toCurrency)
        {
            return _converter.Convert(amount, fromCurrency, toCurrency);
        }
    }
}
