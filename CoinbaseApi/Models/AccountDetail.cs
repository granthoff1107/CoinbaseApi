using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Models
{
    public class AccountDetail
    {
        public Account Account { get; set; }
        public List<AccountAddress> Addresses { get; set; }
    }
}
