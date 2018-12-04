using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.RateLimit
{
    public struct ClientRateLimitCounter
    {
        [JsonConstructor]
        public ClientRateLimitCounter(DateTime timestamp, long totalRequests)
        {
            Timestamp = timestamp;
            TotalRequests = totalRequests;
        }

        /// <summary>
        /// 最后请求时间
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// 请求总数
        /// </summary>
        public long TotalRequests { get; private set; }
    }
}
