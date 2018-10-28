using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class AccountAddress
    {
        public string Id { get; set; }
        public string Address { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}


