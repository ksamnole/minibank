using Minibank.Core.Domains.BankAccounts.Services;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
       float Convert(float amount, AllowedCurrency fromCurrency, AllowedCurrency toCurrency);
    }
}
