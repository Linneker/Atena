using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using Microsoft.Extensions.Options;
using NFeEntity = Acme.Sistemas.Domain.Entities.Fiscal.NFe;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirNFe;

public sealed class EmitirNFeCommandHandler
    : IRequestHandler<EmitirNFeCommand, ResponseDefault<EmitirNFeCommandResult>>
{
    private readonly IConfiguracaoFiscalRepository _config;
    private readonly INFeRepository _nfes;
    private readonly ITenantRepository _tenants;
    private readonly INFeTransmissaoEnqueuer _enqueuer;
    private readonly FiscalOptions _options;
    private readonly ITenantContext _tenantContext;

    public EmitirNFeCommandHandler(
        IConfiguracaoFiscalRepository config,
        INFeRepository nfes,
        ITenantRepository tenants,
        INFeTransmissaoEnqueuer enqueuer,
        IOptions<FiscalOptions> options,
        ITenantContext tenantContext)
    {
        _config = config;
        _nfes = nfes;
        _tenants = tenants;
        _enqueuer = enqueuer;
        _options = options.Value;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<EmitirNFeCommandResult>> Handle(EmitirNFeCommand request, CancellationToken cancellationToken)
    {
        var config = await _config.GetAsync(cancellationToken);
        if (config is null)
            return ResponseDefault<EmitirNFeCommandResult>.Conflict(
                "Configuração fiscal não encontrada. Configure ambiente, CNPJ emitente e certificado primeiro.");
        if (config.CertificadoPfxCriptografado is null || config.CertificadoPfxCriptografado.Length == 0)
            return ResponseDefault<EmitirNFeCommandResult>.Conflict(
                "Certificado A1 não importado. Importe o PFX antes de emitir NF-e.");

        // Limite por plano
        var limites = await _tenants.GetLimitesAsync(_tenantContext.TenantId, cancellationToken);
        var limite = limites?.MaxNFeMes ?? _options.LimitePadraoNFePorMes;
        if (limite > 0)
        {
            var hoje = DateTime.UtcNow;
            var emitidasMes = await _nfes.CountAutorizadasNoMesAsync(hoje.Year, hoje.Month, cancellationToken);
            if (emitidasMes >= limite)
                return ResponseDefault<EmitirNFeCommandResult>.Conflict(
                    $"Limite de NF-e do plano atingido: {emitidasMes}/{limite} no mês. Faça upgrade do plano.");
        }

        var numero = await _config.ReservarProximoNumeroAsync(config.SerieNFe, cancellationToken);
        var dataEmissao = DateTime.UtcNow;
        var cUf = NFeChaveAcessoBuilder.CodigoUf(config.Uf);
        var codigoNumerico = Random.Shared.Next(10_000_000, 99_999_999);
        var tpEmis = (int)config.Modo;
        var chave = NFeChaveAcessoBuilder.Build(cUf, dataEmissao, config.CnpjEmitente, config.SerieNFe, numero, tpEmis, codigoNumerico);

        var valorTotal = request.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

        var nfe = new NFeEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            Serie = config.SerieNFe,
            ChaveAcesso = chave,
            FaturamentoId = request.FaturamentoId,
            ClienteId = request.ClienteId,
            Ambiente = config.Ambiente,
            Modo = config.Modo,
            DataEmissao = dataEmissao,
            Status = StatusNFe.AguardandoTransmissao,
            ValorTotal = valorTotal,
            CreatedBy = _tenantContext.UserId
        };
        await _nfes.AddAsync(nfe, cancellationToken);

        var nItem = 1;
        var itens = request.Itens.Select(i => new NFeItem
        {
            TenantId = _tenantContext.TenantId,
            NFeId = nfe.Id,
            NumeroItem = nItem++,
            ProdutoId = i.ProdutoId,
            Descricao = i.Descricao,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Cfop = i.Cfop,
            Ncm = i.Ncm,
            CreatedBy = _tenantContext.UserId
        }).ToList();
        await _nfes.AddItensAsync(itens, cancellationToken);

        // Enfileira transmissão assíncrona
        await _enqueuer.EnqueueAsync(_tenantContext.TenantId, nfe.Id, cancellationToken);

        return ResponseDefault<EmitirNFeCommandResult>.Created(
            new EmitirNFeCommandResult(nfe.Id, numero, config.SerieNFe, chave, EnfileiradaParaTransmissao: true));
    }
}
