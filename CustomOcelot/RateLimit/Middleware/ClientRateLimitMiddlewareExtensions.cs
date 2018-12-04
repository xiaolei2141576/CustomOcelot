using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.RateLimit.Middleware
{
    /// <summary>
    /// 限流中间件扩展
    /// </summary>
    public static class ClientRateLimitMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseClientRateLimitMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<ClientRateLimitMiddleware>();
        }
    }
}
