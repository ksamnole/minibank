using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
       float Convert(float amount, Currency fromCurrency, Currency toCurrency);
    }
}
