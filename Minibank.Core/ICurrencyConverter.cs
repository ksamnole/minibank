using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
       float Convert(float amountInRubles, string currencyCode);
    }
}
