using Microsoft.Extensions.DependencyInjection;
using Services.Redis.Service;

namespace Services.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCache( this IServiceCollection services,string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "RedisProj";
        });

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}