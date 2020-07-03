using CustomOcelot.Authentication;
using CustomOcelot.Cache;
using CustomOcelot.Configuration;
using CustomOcelot.Configuration.Model;
using CustomOcelot.DataBase;
using CustomOcelot.RateLimit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Responder;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomOcelot.Middleware
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加默认的注入方式，所有需要传入的参数都是用默认值 默认使用sqlserver
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddCustomOcelot(this IOcelotBuilder builder, Action<OcelotConfiguration> option)
        {
            builder.Services.Configure(option);
            //配置信息
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<OcelotConfiguration>>().Value);
            //注册后端服务
            builder.Services.AddHostedService<DbConfigurationPoller>();
            //使用Redis重写缓存
            //为了减少后端请求，在数据库提取的方法前都加入了缓存，把用到的接口添加到入口进行注入。
            builder.Services.AddSingleton<IOcelotCache<FileConfiguration>, InRedisCache<FileConfiguration>>();
            builder.Services.AddSingleton<IOcelotCache<CachedResponse>, InRedisCache<CachedResponse>>();
            builder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();
            builder.Services.AddSingleton<IOcelotCache<ClientRoleModel>, InRedisCache<ClientRoleModel>>();
            builder.Services.AddSingleton<IOcelotCache<RateLimitRuleModel>, InRedisCache<RateLimitRuleModel>>();
            builder.Services.AddSingleton<IOcelotCache<ClientRateLimitCounter?>, InRedisCache<ClientRateLimitCounter?>>();
            //注入授权
            builder.Services.AddSingleton<IAuthenticationProcessor, AuthenticationProcessor>();
            //注入限流实现
            builder.Services.AddSingleton<IClientRateLimitProcessor, ClientRateLimitProcessor>();
            //重写错误状态码
            builder.Services.AddSingleton<IErrorsToHttpStatusCodeMapper, ErrorsToHttpStatusCodeMapper>();
            return builder;
        }

        /// <summary>
        /// 扩展使用Mysql存储。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder UseMySql(this IOcelotBuilder builder)
        {
            //配置文件仓储注入使用mysql
            builder.Services.AddSingleton<IFileConfigurationRepository, MySqlFileConfigurationRepository>();
            builder.Services.AddSingleton<IClientAuthenticationRepository, MySqlClientAuthenticationRepository>();
            builder.Services.AddSingleton<IClientRateLimitRepository, MySqlClientRateLimitRepository>();
            return builder;
        }
        /// <summary>
        /// 扩展使用Mysql存储。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder UseSqlServer(this IOcelotBuilder builder)
        {
            //配置文件仓储注入使用sqlserver
            builder.Services.AddSingleton<IFileConfigurationRepository, SqlServerFileConfigurationRepository>();
            builder.Services.AddSingleton<IClientAuthenticationRepository, SqlServerClientAuthenticationRepository>();
            builder.Services.AddSingleton<IClientRateLimitRepository, SqlServerClientRateLimitRepository>();
            return builder;
        }
    }
}
