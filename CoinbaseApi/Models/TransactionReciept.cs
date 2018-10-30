using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class TransactionReciept
    {
        public string Id { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public Balance Amount { get; set; }
        [JsonProperty("native_amount")]
        public Balance NativeAmount { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public string Idem { get; set; }
        [JsonProperty("instant_exchange")]
        public bool InstantExchange { get; set; }
        public ToAddress To { get; set; }
        public string Description { get; set; }
    }
}
