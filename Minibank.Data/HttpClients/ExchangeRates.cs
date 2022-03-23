using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Data.HttpClients.Models;
using System.Net.Http;
using System.Net.Http.Json;

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
                : throw new ValidationException($"Неправильный код валюты", code);
        }
    }
}
