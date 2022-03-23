using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
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
        public float Convert(int amount, AllowedCurrency fromCurrency, AllowedCurrency toCurrency)
        {
            return _converter.Convert(amount, fromCurrency, toCurrency);
        }
    }
}
