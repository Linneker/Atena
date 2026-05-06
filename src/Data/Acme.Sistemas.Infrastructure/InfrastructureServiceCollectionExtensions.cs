using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Infrastructure.AppConfiguration;
using Acme.Sistemas.Infrastructure.Cache;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Infrastructure.Ged;
using Acme.Sistemas.Infrastructure.Messaging.Email;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Acme.Sistemas.Repository.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddAcmeInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RetryOptions>(configuration.GetSection(RetryOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<FeatureFlagSettings>(configuration.GetSection(FeatureFlagSettings.SectionName));

        services.AddSingleton<RetryPolicy>();
        services.AddScoped<IDataConfiguration, DataConfiguration>();
        services.AddScoped<TransactionManager>();

        services.AddSingleton<ICacheStore, CacheStore>();
        services.AddSingleton<IRabbitMqBus, RabbitMqBus>();
        services.AddScoped<IEmailQueueService, EmailQueueService>();

        services.AddSingleton<IGedStorageProvider>(sp =>
            new GedLocalStorageProvider(Path.Combine(AppContext.BaseDirectory, "ged-local")));
        services.AddSingleton<IGedDocumentStorageProviderResolver, GedDocumentStorageProviderResolver>();

        services.AddSingleton<IFeatureFlagService, FeatureFlagService>();

        var redisConn = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConn))
        {
            services.AddStackExchangeRedisCache(opts => opts.Configuration = redisConn);
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        return services;
    }
}
