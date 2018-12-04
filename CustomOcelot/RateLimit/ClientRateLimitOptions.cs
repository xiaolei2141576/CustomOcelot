using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.RateLimit
{
    /// <summary>
    /// 限流相关选项 配置
    /// </summary>
    public class ClientRateLimitOptions
    {
        /// <summary>
        /// 限流请求的地址
        /// </summary>
        public string RateLimitPath { get; set; }

        /// <summary>
        /// 限制的访问次数
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 限流的策略，如  1s 2m 3h 4d
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }
    }
}
