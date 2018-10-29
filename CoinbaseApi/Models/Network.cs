using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class Network
    {
        public NetworkStatus Status { get; set; }
        public string Hash { get; set; }
        [JsonProperty("transaction_fee")]
        public Balance TransactionFee { get; set; }
        [JsonProperty("transaction_amount")]
        public Balance TransactionAmount { get; set; }
        public int Confirmations { get; set; }
    }
}
