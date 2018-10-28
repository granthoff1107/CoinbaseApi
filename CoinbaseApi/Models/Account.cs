using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public enum Currency
    {
        BTC = 1,
        BCH,
        ETC,
        ETH,
        LTC,
        USDC,
        ZRX
    }

    public enum AccountType
    {
        Wallet,
    }

    public class Balance
    {
        public Double Amount { get; set; }
        public Currency Currency { get; set; }
    }

    public class CoinbaseResponseWrapper<T>
    {
        public T Data { get; set; }
    }

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
