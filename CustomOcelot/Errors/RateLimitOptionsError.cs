using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Errors
{
    public class RateLimitOptionsError : Error
    {
        public RateLimitOptionsError(string message, int httpStatusCode) : base(message, OcelotErrorCode.RateLimitOptionsError, httpStatusCode)
        {

        }
    }
}
