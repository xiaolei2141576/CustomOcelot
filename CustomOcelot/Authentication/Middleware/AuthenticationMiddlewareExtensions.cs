using Microsoft.AspNetCore.Builder;
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
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
