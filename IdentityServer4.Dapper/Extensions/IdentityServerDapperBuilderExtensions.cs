using IdentityServer4.Dapper.HostedServices;
using IdentityServer4.Dapper.Interfaces;
using IdentityServer4.Dapper.Options;
using IdentityServer4.Dapper.Stores.MySql;
using IdentityServer4.Dapper.Stores.SqlServer;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Dapper.Extensions
{
    /// <summary>
    /// 使用Dapper扩展
    /// </summary>
    public static class IdentityServerDapperBuilderExtensions
    {
        /// <summary>
        /// 配置Dapper接口和实现(默认使用SqlServer)
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">存储配置信息</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddDapperStore(
            this IIdentityServerBuilder builder,
            Action<DapperStoreOptions> storeOptionsAction = null)
        {
            var options = new DapperStoreOptions();
            builder.Services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);
            builder.Services.AddTransient<IClientStore, SqlServerClientStore>();
            builder.Services.AddTransient<IResourceStore, SqlServerResourceStore>();
            builder.Services.AddTransient<IPersistedGrantStore, SqlServerPersistedGrantStore>();
            builder.Services.AddTransient<IPersistedGrants, SqlServerPersistedGrants>();
            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();
            return builder;
        }

        /// <summary>
        /// 使用Mysql存储
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder UseMySql(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, MySqlClientStore>();
            builder.Services.AddTransient<IResourceStore, MySqlResourceStore>();
            builder.Services.AddTransient<IPersistedGrantStore, MySqlPersistedGrantStore>();
            builder.Services.AddTransient<IPersistedGrants, MySqlPersistedGrants>();
            return builder;
        }
    }
}
