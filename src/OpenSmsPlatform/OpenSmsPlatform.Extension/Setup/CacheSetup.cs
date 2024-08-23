using Microsoft.Extensions.DependencyInjection;
using OpenSmsPlatform.Common.Caches;
using OpenSmsPlatform.Common.Core;
using OpenSmsPlatform.Common.Option;
using OpenSmsPlatform.Extension.Redis;
using StackExchange.Redis;

namespace OpenSmsPlatform.Extension
{
    public static class CacheSetup
    {
        public static void AddCacheSetup(this IServiceCollection services)
        {
            var cacheOptions = App.GetOptions<RedisOptions>();
            if (cacheOptions.Enable) //使用Redis缓存
            {
                // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    //获取连接字符串
                    var configuration = ConfigurationOptions.Parse(cacheOptions.ConnectionString, true);
                    configuration.ResolveDns = true;
                    return ConnectionMultiplexer.Connect(configuration);
                });
                services.AddSingleton<ConnectionMultiplexer>(p => p.GetService<IConnectionMultiplexer>() as ConnectionMultiplexer);

                //使用Redis
                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(App.GetService<IConnectionMultiplexer>(false));
                    if (!string.IsNullOrEmpty(cacheOptions.InstanceName)) options.InstanceName = cacheOptions.InstanceName;
                });

                services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();
            }
            else             //使用内存缓存
            {
                services.AddMemoryCache();
                services.AddDistributedMemoryCache();
            }

            services.AddSingleton<ICaching, Caching>();
        }
    }
}
