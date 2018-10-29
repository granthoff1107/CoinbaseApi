using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Primary { get; set; }
        public AccountType Type { get; set; }
        public Balance Balance { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
