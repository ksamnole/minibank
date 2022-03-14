using Minibank.Core;
using Minibank.Data.HttpClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minibank.Data
{
    public class ExchangeRates : IExchangeRates
    {
        private readonly HttpClient httpClient;

        public ExchangeRates(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public float GetExchange(string code)
        {
            var response = httpClient.GetFromJsonAsync<ExchangeResponse>("daily_json.js")
                .GetAwaiter().GetResult();

            return response.Valute.TryGetValue(code, out var valute)
                ? valute.Value
                : throw new ValidationException($"Не правильный код валюты", code);
        }
    }
}
