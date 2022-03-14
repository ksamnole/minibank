using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Minibank.Data.HttpClients.Models
{
    public class ExchangeResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, ValueItem> Valute { get; set; }
    }

    public class ValueItem
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }
        public string NumCode { get; set; }
        public float Value { get; set; }
    }
}
