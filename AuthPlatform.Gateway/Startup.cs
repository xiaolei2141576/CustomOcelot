using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomOcelot.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IdentityServer4.AccessTokenValidation;
using Ocelot.Administration;

namespace AuthPlatform.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "TestKey";
            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = "http://localhost:6611"; //IdentityServer地址
                o.RequireHttpsMetadata = false;
                o.ApiName = "gateway"; //网关管理的名称，对应的为客户端授权的scope
            };
            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options);
            //注册ocelot服务
            services.AddOcelot().AddCustomOcelot(option =>
            {
                option.DbConnectionStrings = "Server=.;Database=Gateway;User ID=sa;Password=123456789;";
                option.RedisDbConnectionStrings = new List<string>() { "193.112.82.53:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,connectTimeout=1000,connectRetry=1;" };
                option.EnableTimer = true;
                //option.TimerDelay = 5 * 1000; 
                option.ClientAuthorization = true;
            }).AddAdministration("/CustomOcelot", options);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseCustomOcelot().Wait();
            app.UseMvc();
        }
    }
}
