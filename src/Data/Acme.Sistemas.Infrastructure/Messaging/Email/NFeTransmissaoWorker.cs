using System.Security.Cryptography;
using System.Text;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Ged;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class NFeTransmissaoWorker : BackgroundService
{
    private readonly IRabbitMqBus _bus;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NFeTransmissaoWorker> _logger;

    public NFeTransmissaoWorker(
        IRabbitMqBus bus,
        IServiceScopeFactory scopeFactory,
        ILogger<NFeTransmissaoWorker> logger)
    {
        _bus = bus;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var binding = new SubscribeBinding(
            Exchange: NFeQueueNames.Exchange,
            Queue: NFeQueueNames.Queue,
            RoutingKey: NFeQueueNames.RoutingKey,
            DeadLetterExchange: NFeQueueNames.DeadLetterExchange,
            DeadLetterQueue: NFeQueueNames.DeadLetterQueue,
            DeadLetterRoutingKey: NFeQueueNames.DeadLetterRoutingKey,
            MaxRetries: 5,
            PrefetchCount: 4);

        try
        {
            await _bus.SubscribeBoundAsync<NFeTransmissaoMessage>(
                binding, ProcessAsync, stoppingToken);
            _logger.LogInformation("NFeTransmissaoWorker pronto na fila {Queue}.", binding.Queue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao iniciar NFeTransmissaoWorker.");
        }

        await Task.Delay(Timeout.Infinite, stoppingToken).ContinueWith(_ => { });
    }

    private async Task ProcessAsync(NFeTransmissaoMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        // Substitui o tenant context no escopo do worker.
        var mutableTenant = scope.ServiceProvider.GetRequiredService<IMutableTenantContext>();
        mutableTenant.Override(message.TenantId);

        var configRepo = scope.ServiceProvider.GetRequiredService<IConfiguracaoFiscalRepository>();
        var nfeRepo = scope.ServiceProvider.GetRequiredService<INFeRepository>();
        var clienteRepo = scope.ServiceProvider.GetRequiredService<IClienteRepository>();
        var tenantRepo = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
        var xmlBuilder = scope.ServiceProvider.GetRequiredService<INFeXmlBuilder>();
        var signer = scope.ServiceProvider.GetRequiredService<INFeXmlSigner>();
        var sefaz = scope.ServiceProvider.GetRequiredService<INFeSefazClient>();
        var cipher = scope.ServiceProvider.GetRequiredService<TenantSecretCipher>();
        var storage = scope.ServiceProvider.GetRequiredService<IGedDocumentStorageProviderResolver>();

        var nfe = await nfeRepo.GetByIdAsync(message.NFeId, cancellationToken);
        if (nfe is null) return;

        var config = await configRepo.GetAsync(cancellationToken);
        if (config is null || config.CertificadoPfxCriptografado is null)
        {
            await nfeRepo.UpdateStatusAsync(nfe.Id, StatusNFe.Rejeitada, "999",
                "Configuração fiscal ausente ou sem certificado.", null, null, null, null, cancellationToken);
            return;
        }

        var tenant = await tenantRepo.GetByIdAsync(message.TenantId, cancellationToken);
        var emitenteRazao = config.RazaoSocialEmitente ?? tenant?.RazaoSocial ?? "Atena";

        var itens = await nfeRepo.ListItensAsync(nfe.Id, cancellationToken);
        var xmlEnvio = xmlBuilder.BuildEnvio(nfe, itens, config, emitenteRazao);

        // Decifra PFX e senha
        var pfx = cipher.Decrypt(config.CertificadoPfxCriptografado, config.CertificadoNonceBase64!, message.TenantId);
        var senhaCipher = Convert.FromBase64String(config.CertificadoSenhaCriptografada!);
        var senha = Encoding.UTF8.GetString(cipher.Decrypt(senhaCipher, config.CertificadoSenhaNonceBase64!, message.TenantId));

        var xmlAssinado = signer.Sign(xmlEnvio, pfx, senha);
        var hashEnviado = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(xmlAssinado))).ToLowerInvariant();

        await nfeRepo.UpdateStatusAsync(nfe.Id, StatusNFe.Transmitindo, null, null, null, null, null, null, cancellationToken);

        SefazResultado resultado;
        try
        {
            resultado = await sefaz.AutorizarAsync(xmlAssinado, nfe.Ambiente, config.Uf, config.Modo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha na transmissão SEFAZ — ativando contingência SVRS para NF-e {Id}", nfe.Id);
            // Contingência: alterna modo e reenfileira
            await SwitchToContingenciaAsync(configRepo, config, cancellationToken);
            await nfeRepo.UpdateStatusAsync(nfe.Id, StatusNFe.EmContingencia, "VVVV",
                "Falha de comunicação — contingência SVRS ativada.", null, null, null, null, cancellationToken);
            await _bus.PublishAsync(NFeQueueNames.Exchange, NFeQueueNames.RoutingKey, message, cancellationToken);
            return;
        }

        if (resultado.Sucesso)
        {
            // Armazena XML autorizado em GED (provider configurável: Local ou AwsS3)
            var fiscalOptions = scope.ServiceProvider.GetRequiredService<IOptions<FiscalOptions>>().Value;
            var ged = storage.Resolve(fiscalOptions.XmlStorageProvider);
            var path = $"{message.TenantId}/{nfe.DataEmissao:yyyy}/{nfe.DataEmissao:MM}/{nfe.ChaveAcesso}.xml";
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlAssinado));
            var url = await ged.UploadAsync(path, ms, "application/xml", cancellationToken);

            await nfeRepo.UpdateStatusAsync(
                nfe.Id, StatusNFe.Autorizada,
                resultado.Codigo, resultado.Motivo, resultado.Protocolo, resultado.DataAutorizacao,
                nfe.ChaveAcesso, url, cancellationToken);

            _logger.LogInformation("NF-e autorizada: chave={Chave} protocolo={Prot}",
                nfe.ChaveAcesso, resultado.Protocolo);
        }
        else
        {
            await nfeRepo.UpdateStatusAsync(
                nfe.Id, StatusNFe.Rejeitada,
                resultado.Codigo, resultado.Motivo, null, null, null, null, cancellationToken);
        }
    }

    private static async Task SwitchToContingenciaAsync(IConfiguracaoFiscalRepository repo, ConfiguracaoFiscal config, CancellationToken ct)
    {
        if (config.Modo != ModoTransmissao.ContingenciaSvrs)
        {
            config.Modo = ModoTransmissao.ContingenciaSvrs;
            await repo.UpsertAsync(config, ct);
        }
    }
}

