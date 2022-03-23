using Minibank.Core.Domains.BankAccounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRates _exchangeRates;

        public CurrencyConverter(IExchangeRates exchangeRates)
        {
            this._exchangeRates = exchangeRates;
        }

        public float Convert(float amount, AllowedCurrency fromCurrency, AllowedCurrency toCurrency)
        {
            if (amount < 0)
                throw new ValidationException("Сумма должна быть больше 0", amount);

            var value = amount;

            if (fromCurrency != AllowedCurrency.RUB)
                value *= _exchangeRates.GetExchange(fromCurrency.ToString());

            if (toCurrency != AllowedCurrency.RUB)
                value /= _exchangeRates.GetExchange(toCurrency.ToString());

            return value;
        }
    }
}
