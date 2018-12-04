using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Errors
{
    public class RateLimitOptionsError : Error
    {
        public RateLimitOptionsError(string message) : base(message, OcelotErrorCode.RateLimitOptionsError)
        {

        }
    }
}
