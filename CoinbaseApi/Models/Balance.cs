using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class Balance
    {
        public Double Amount { get; set; }
        public Currency Currency { get; set; }
    }
}
