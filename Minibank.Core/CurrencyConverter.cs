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

        public float Convert(float amountInRubles, string currencyCode)
        {
            var value = amountInRubles * exchangeRates.GetExchange(currencyCode);
            return value >= 0 ? value : throw new UserFriendlyException("Отрицательное значение", value);
        }
    }
}
