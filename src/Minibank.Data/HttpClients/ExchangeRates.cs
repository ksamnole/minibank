using Minibank.Core;
using Minibank.Data.HttpClients.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Minibank.Data
{
    public class ExchangeRates : IExchangeRates
    {
        private readonly HttpClient _httpClient;

        public ExchangeRates(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public float GetExchange(string code)
        {
            var response = _httpClient.GetFromJsonAsync<ExchangeResponse>("daily_json.js")
                .GetAwaiter().GetResult();

            return response.Valute.TryGetValue(code, out var valute)
                ? valute.Value
                : throw new ValidationException($"Неправильный код валюты", code);
        }
    }
}
