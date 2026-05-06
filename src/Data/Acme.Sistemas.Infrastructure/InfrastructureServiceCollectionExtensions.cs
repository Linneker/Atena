using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Infrastructure.AppConfiguration;
using Acme.Sistemas.Infrastructure.Cache;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Infrastructure.Ged;
using Acme.Sistemas.Infrastructure.Messaging.Email;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Acme.Sistemas.Infrastructure.Reports;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using Acme.Sistemas.Services.V1.Relatorios.Pdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        services.AddSingleton<ICacheStore, CacheStore>();
        services.AddSingleton<IRabbitMqBus, RabbitMqBus>();
        services.AddScoped<IEmailQueueService, EmailQueueService>();
        services.AddScoped<ISmtpEmailSender, MailKitSmtpEmailSender>();
        services.AddHostedService<EmailDispatcherHostedService>();

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

        services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
        services.AddSingleton<IRelatorioPdfRenderer, QuestPdfRelatorioRenderer>();
        services.AddSingleton<IPedidoCompraPdfRenderer, QuestPdfPedidoCompraRenderer>();

        // Fiscal NF-e
        services.AddSingleton<TenantSecretCipher>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<FiscalOptions>>().Value;
            return new TenantSecretCipher(opts.MasterEncryptionKey);
        });
        services.AddSingleton<INFeXmlBuilder, NFeXmlBuilder>();
        services.AddSingleton<INFeXmlSigner, StubNFeXmlSigner>();
        services.AddSingleton<INFeSefazClient, StubNFeSefazClient>();
        services.AddSingleton<INFeTransmissaoEnqueuer, NFeTransmissaoEnqueuer>();
        services.AddSingleton<IDanfePdfRenderer, QuestPdfDanfeRenderer>();
        services.AddSingleton<Services.V1.Relatorios.Export.IRelatorioExporter, RelatorioExporter>();
        services.AddHostedService<NFeTransmissaoWorker>();
        services.AddHostedService<Hosted.CertificadoVencimentoVarreduraWorker>();

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
