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
        public override string Message => "Fobidden, Check your scopes to make sure you have Access Priviledges";
    }

}
