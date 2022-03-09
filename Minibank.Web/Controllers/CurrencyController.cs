﻿using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyConverter converter;

        public CurrencyController(ICurrencyConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet("{amountInRubles}/{currencyCode}")]
        public float Get(int amountInRubles, string currencyCode)
        {
            return converter.Convert(amountInRubles, currencyCode);
        }
    }
}
