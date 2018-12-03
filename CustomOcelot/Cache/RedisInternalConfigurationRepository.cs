using CustomOcelot.Configuration;
using CustomOcelot.Extension;
using Ocelot.Configuration;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Cache
{
    public class RedisInternalConfigurationRepository : IInternalConfigurationRepository
    {
        /// <summary>
        /// 使用redis存储内部配置信息
        /// </summary>
        private readonly OcelotConfiguration _options;
        private IInternalConfiguration _internalConfiguration;
        public RedisInternalConfigurationRepository(OcelotConfiguration options)
        {
            _options = options;
            CSRedis.CSRedisClient csredis;
            if (options.RedisDbConnectionStrings.Count == 1)
            {
                //普通模式
                csredis = new CSRedis.CSRedisClient(options.RedisDbConnectionStrings[0]);
            }
            else
            {
                //集群模式
                //实现思路：根据key.GetHashCode() % 节点总数量，确定连向的节点
                //也可以自定义规则(第一个参数设置)
                csredis = new CSRedis.CSRedisClient(null, options.RedisDbConnectionStrings.ToArray());
            }
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }
        public Response AddOrReplace(IInternalConfiguration internalConfiguration)
        {
            var key = _options.RedisKeyPrefix + "-internalConfiguration";
            RedisHelper.Set(key, internalConfiguration.ToJson());
            return new OkResponse();
        }

        public Response<IInternalConfiguration> Get()
        {
            var key = _options.RedisKeyPrefix + "-internalConfiguration";
            var result = RedisHelper.Get<InternalConfiguration>(key);
            if (result != null)
            {
                return new OkResponse<IInternalConfiguration>(result);
            }
            return new OkResponse<IInternalConfiguration>(default(InternalConfiguration));
        }
    }
}
