using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Shared.Helpers.ElasticSearchLogs;

public static class SerilogConfiguration
{
    public static IServiceCollection AddElasticSerilog(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var elasticUri = new Uri(configuration["ElasticSettings:Uri"]!);
        var elasticUser = configuration["ElasticSettings:UserName"];
        var elasticPass = configuration["ElasticSettings:Password"];

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(
                new[] { elasticUri },
                opts => {  },
                transport =>
                {
                    transport.Authentication(
                        new BasicAuthentication(elasticUser ?? string.Empty, elasticPass ?? string.Empty));
                })
            .CreateLogger();

        return services;
    }
}