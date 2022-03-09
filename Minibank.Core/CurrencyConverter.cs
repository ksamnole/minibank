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
            if (amountInRubles < 0)
                throw new UserFriendlyException("Отрицательное значение", amountInRubles);
            var value = amountInRubles * exchangeRates.GetExchange(currencyCode);
            return value;
        }
    }
}
