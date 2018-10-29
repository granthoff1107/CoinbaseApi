using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Exceptions
{
    public class CoinbaseException : Exception
    {
    }

    public class TokenExpiredException : CoinbaseException
    {
        public override string Message => "Token Has Expired";
    }

    public class InvalidScopeException : CoinbaseException
    {
        public override string Message => "Forbidden, Check your scopes to make sure you have Access Priviledges";
    }

    public class Transaction2FaRequiredException : CoinbaseException
    {
        public override string Message => "Two factor authentication is required, replay the request with the 2FA Token";
    }

}
