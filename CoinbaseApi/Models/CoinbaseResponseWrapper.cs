using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class CoinbaseResponseWrapper<T>
    {
        public T Data { get; set; }
    }
}
