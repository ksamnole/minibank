using Minibank.Core.Domains.BankAccounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface IExchangeRates
    {
        float GetExchange(string code);
    }
}
