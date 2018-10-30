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
        USD,
        ZRX
    }

    public enum AccountType
    {
        Wallet,
    }

    public enum NetworkStatus
    {
        Confirmed,
    }

    public enum TransactionStatus
    {
        Pending,
        Complete,
        Failed,
        Expired,
        Canceled
    }

    public enum TransactionType
    {
        Send,
        Transfer,
        Request
    }

}
