using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Authentication.Middleware
{
    /// <summary>
    /// 使用自定义授权中间件
    /// </summary>
    public static class AuthenticationMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseAuthenticationMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
