using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRates exchangeRates;

        public CurrencyConverter(IExchangeRates exchangeRates)
        {
            this.exchangeRates = exchangeRates;
        }

        public float Convert(float amount, string fromCurrency, string toCurrency)
        {
            if (amount < 0)
                throw new ValidationException("Отрицательное значение", amount);
            if (fromCurrency == null || toCurrency == null)
                throw new NullReferenceException();

            var value = amount;

            if (fromCurrency != "RUB")
                value *= exchangeRates.GetExchange(fromCurrency);

            if (toCurrency != "RUB")
                value /= exchangeRates.GetExchange(toCurrency);

            return value;
        }
    }
}
