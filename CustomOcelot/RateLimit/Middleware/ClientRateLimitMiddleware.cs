using CustomOcelot.Configuration;
using CustomOcelot.Errors;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomOcelot.RateLimit.Middleware
{
    public class ClientRateLimitMiddleware : OcelotMiddleware
    {
        private readonly OcelotConfiguration _options;
        private readonly IClientRateLimitProcessor _clientRateLimitProcessor;
        private readonly RequestDelegate _next;
        public ClientRateLimitMiddleware(RequestDelegate next,
            IOcelotLoggerFactory loggerFactory,
            IClientRateLimitProcessor clientRateLimitProcessor,
            OcelotConfiguration options)
            : base(loggerFactory.CreateLogger<ClientRateLimitMiddleware>())
        {
            _next = next;
            _clientRateLimitProcessor = clientRateLimitProcessor;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientId = "client_cjy"; //使用默认客户端

            if (!_options.ClientRateLimit)
            {
                Logger.LogInformation($"未启用客户端限流中间件");
            }
            else
            {
                //非认证的渠道
                if (!context.Items.DownstreamRoute().IsAuthenticated)
                {
                    if (context.Request.Headers.Keys.Contains(_options.ClientKey))
                    {
                        clientId = context.Request.Headers[_options.ClientKey].First();
                    }
                }
                else //认证的渠道，从Claim中提取
                {
                    var clientClaim = context.User.Claims.FirstOrDefault(p => p.Type == _options.ClientKey);
                    if (string.IsNullOrWhiteSpace(clientClaim?.Value))
                    {
                        clientId = clientClaim?.Value;
                    }
                }
                //路由地址
                var path = context.Items.DownstreamRoute().UpstreamPathTemplate.OriginalValue;
                //1、校验路由是否有限流策略
                //2、校验客户端是否被限流了
                //3、校验客户端是否启动白名单
                //4、校验是否触发限流及计数
                if (await _clientRateLimitProcessor.CheckClientRateLimitResultAsync(clientId, path))
                {
                    await _next.Invoke(context);
                }
                else
                {
                    var error = new RateLimitOptionsError($"请求路由 {context.Request.Path}触发限流策略", (int)HttpStatusCode.TooManyRequests);
                    Logger.LogWarning($"路由地址 {context.Request.Path} 触发限流策略. {error}");
                    context.Items.SetError(error);
                }
            }
        }
    }
}
