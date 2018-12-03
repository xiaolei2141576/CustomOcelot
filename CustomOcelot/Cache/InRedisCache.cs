using CustomOcelot.Configuration;
using CustomOcelot.Extension;
using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Cache
{
    /// <summary>
    /// 使用redis重写缓存
    /// </summary>
    public class InRedisCache<T> : IOcelotCache<T>
    {
        private readonly OcelotConfiguration _options;
        public InRedisCache(OcelotConfiguration options)
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

        /// <summary>
        /// 添加缓存信息
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的实体</param>
        /// <param name="ttl">过期时间</param>
        /// <param name="region">缓存所属分类，可以指定分类缓存过期</param>
        public void Add(string key, T value, TimeSpan ttl, string region)
        {
            key = GetKey(region, key);
            if (ttl.TotalMilliseconds <= 0)
            {
                return;
            }
            RedisHelper.Set(key, value.ToJson(), (int)ttl.TotalSeconds);
        }

        /// <summary>
        /// 添加缓存信息
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的实体</param>
        /// <param name="ttl">过期时间</param>
        /// <param name="region">缓存所属分类，可以指定分类缓存过期</param>
        public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
        {
            Add(key, value, ttl, region);
        }

        /// <summary>
        /// 批量移除regin开头的所有缓存记录
        /// </summary>
        /// <param name="region">缓存分类</param>
        public void ClearRegion(string region)
        {
            //获取所有满足条件的key
            var data = RedisHelper.Keys(_options.RedisKeyPrefix + "-" + region + "-*");
            //批量删除
            RedisHelper.Del(data);
        }

        public T Get(string key, string region)
        {
            key = GetKey(region, key);
            var result = RedisHelper.Get(key);
            if (!string.IsNullOrEmpty(result))
            {
                return result.ToObject<T>();
            }
            return default(T);
        }

        /// <summary>
        /// 获取格式化后的key
        /// </summary>
        /// <param name="region">分类标识</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        private string GetKey(string region, string key)
        {
            return _options.RedisKeyPrefix + "-" + region + "-" + key;
        }
    }
}
