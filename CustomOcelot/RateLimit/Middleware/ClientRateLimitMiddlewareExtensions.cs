using Microsoft.AspNetCore.Builder;
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
        public static IApplicationBuilder UseClientRateLimitMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientRateLimitMiddleware>();
        }
    }
}
