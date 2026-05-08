using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;
using Acme.Sistemas.Domain.Interfaces.Cache;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Messaging;
using Acme.Sistemas.Infrastructure.AppConfiguration;
using Acme.Sistemas.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Infrastructure.Ged;
using Acme.Sistemas.Infrastructure.Messaging.Email;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Acme.Sistemas.Infrastructure.Reports;
using Acme.Sistemas.Core.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Acme.Sistemas.Domain.Interfaces.Reports;

namespace Acme.Sistemas.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddAcmeInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RetryOptions>(configuration.GetSection(RetryOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionName));
        services.Configure<FeatureFlagSettings>(configuration.GetSection(FeatureFlagSettings.SectionName));
        services.Configure<FiscalOptions>(configuration.GetSection(FiscalOptions.SectionName));

        services.AddSingleton<RetryPolicy>();
        services.AddScoped<IDataConfiguration, DataConfiguration>();
        services.AddScoped<TransactionManager>();

        // ---- Cache híbrido (LiteDB cold + IMemoryCache hot, Redis opcional) ----
        services.AddMemoryCache();

        var flagsBootstrap = configuration.GetSection(FeatureFlagSettings.SectionName).Get<FeatureFlagSettings>() ?? new FeatureFlagSettings();
        services.AddSingleton(sp => new LiteDbCacheStore(
            flagsBootstrap.Cache.LiteDbPath,
            sp.GetService<ILogger<LiteDbCacheStore>>()));
        services.AddSingleton<HybridCacheStore>();

        if (!string.IsNullOrWhiteSpace(flagsBootstrap.Cache.RedisConnection))
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(flagsBootstrap.Cache.RedisConnection!));
            services.AddSingleton<RedisCacheStore>();
        }

        services.AddSingleton<ICacheStore, CacheProviderRouter>();

        services.AddSingleton<IRabbitMqBus, RabbitMqBus>();
        services.AddScoped<IEmailQueueService, EmailQueueService>();
        services.AddScoped<ISmtpEmailSender, MailKitSmtpEmailSender>();
        // EmailDispatcherHostedService registrado em Api/Program.cs (Acme.Sistemas.Atena.Api.Hosted)

        services.AddSingleton<IGedStorageProvider>(sp =>
            new GedLocalStorageProvider(Path.Combine(AppContext.BaseDirectory, "ged-local")));

        // S3 provider opcional (registra somente se bucket configurado)
        var fiscalCfg = configuration.GetSection(FiscalOptions.SectionName).Get<FiscalOptions>() ?? new FiscalOptions();
        if (!string.IsNullOrWhiteSpace(fiscalCfg.AwsS3BucketXmls))
        {
            services.AddSingleton<Amazon.S3.IAmazonS3>(sp => new Amazon.S3.AmazonS3Client());
            services.AddSingleton<IGedStorageProvider>(sp =>
                new GedAwsS3StorageProvider(
                    sp.GetRequiredService<Amazon.S3.IAmazonS3>(),
                    fiscalCfg.AwsS3BucketXmls!));
        }

        services.AddSingleton<IGedDocumentStorageProviderResolver, GedDocumentStorageProviderResolver>();

        services.AddSingleton<IFeatureFlagService>(sp =>
        {
            var env = sp.GetRequiredService<Microsoft.Extensions.Hosting.IHostEnvironment>();
            var cfg = sp.GetRequiredService<IConfiguration>();
            var log = sp.GetRequiredService<ILogger<FeatureFlagService>>();
            var path = Path.Combine(env.ContentRootPath, "featureflags.json");
            return new FeatureFlagService(cfg, log, path);
        });
        services.AddSingleton<IRelatorioPdfRenderer, QuestPdfRelatorioRenderer>();
        services.AddSingleton<IPedidoCompraPdfRenderer, QuestPdfPedidoCompraRenderer>();

        // Fiscal NF-e
        services.AddSingleton<TenantSecretCipher>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<FiscalOptions>>().Value;
            return new TenantSecretCipher(opts.MasterEncryptionKey);
        });
        // INFeXmlBuilder, INFeXmlSigner, INFeSefazClient → registrados em Services.AddAcmeServices
        services.AddSingleton<INFeTransmissaoEnqueuer, NFeTransmissaoEnqueuer>();
        services.AddSingleton<IDanfePdfRenderer, QuestPdfDanfeRenderer>();
        services.AddSingleton<IRelatorioExporter, RelatorioExporter>();
        // HostedServices (NFeTransmissaoWorker, CertificadoVencimentoVarreduraWorker)
        // são registrados em Api/Program.cs (vivem em Acme.Sistemas.Atena.Api.Hosted)

        return services;
    }
}
