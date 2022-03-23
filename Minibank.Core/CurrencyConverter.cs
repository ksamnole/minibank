using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRates _exchangeRates;

        public CurrencyConverter(IExchangeRates exchangeRates)
        {
            this._exchangeRates = exchangeRates;
        }

        public float Convert(float amount, Currency fromCurrency, Currency toCurrency)
        {
            if (amount < 0)
                throw new ValidationException("Сумма должна быть больше 0", amount);

            var value = amount;

            if (fromCurrency != Currency.RUB)
                value *= _exchangeRates.GetExchange(fromCurrency.ToString());

            if (toCurrency != Currency.RUB)
                value /= _exchangeRates.GetExchange(toCurrency.ToString());

            return value;
        }
    }
}
