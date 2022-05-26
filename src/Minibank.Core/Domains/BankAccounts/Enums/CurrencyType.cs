using System.Text.Json.Serialization;

namespace Minibank.Core.Domains.BankAccounts.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        RUB,
        USD,
        EUR
    }
}
