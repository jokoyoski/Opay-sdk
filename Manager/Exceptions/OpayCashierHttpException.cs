using System;
using System.Net;

namespace OpayCashier.Models.Exceptions
{
    public class OpayCashierHttpException : Exception
    {
        public OpayCashierHttpException(HttpStatusCode code, string body)
        {
            Message = $"Received {(int) code}:{body} while making http request";
        }

        public override string Message { get; }
    }
}