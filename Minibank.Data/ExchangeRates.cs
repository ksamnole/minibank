using Minibank.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data
{
    public class ExchangeRates : IExchangeRates
    {
        private readonly Random rnd;

        public ExchangeRates()
        {
            rnd = new Random();
        }

        public float GetExchange(string code)
        {
            return rnd.Next(0, 10);
        }
    }
}
