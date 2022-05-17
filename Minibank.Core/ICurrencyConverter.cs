using Minibank.Core.Domains.BankAccounts.Enums;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
        double Convert(double amount, Currency fromCurrency, Currency toCurrency);
    }
}
