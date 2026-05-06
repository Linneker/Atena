using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Fiscal.Services;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirCCe;

public sealed class EmitirCCeCommandHandler
    : IRequestHandler<EmitirCCeCommand, ResponseDefault<EmitirCCeCommandResult>>
{
    private readonly INFeRepository _nfes;
    private readonly IConfiguracaoFiscalRepository _config;
    private readonly INFeSefazClient _sefaz;
    private readonly ITenantContext _tenantContext;

    public EmitirCCeCommandHandler(
        INFeRepository nfes,
        IConfiguracaoFiscalRepository config,
        INFeSefazClient sefaz,
        ITenantContext tenantContext)
    {
        _nfes = nfes;
        _config = config;
        _sefaz = sefaz;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<EmitirCCeCommandResult>> Handle(EmitirCCeCommand request, CancellationToken cancellationToken)
    {
        var nfe = await _nfes.GetByIdAsync(request.NFeId, cancellationToken);
        if (nfe is null)
            return ResponseDefault<EmitirCCeCommandResult>.NotFound("NF-e não encontrada.");
        if (nfe.Status != StatusNFe.Autorizada)
            return ResponseDefault<EmitirCCeCommandResult>.Conflict(
                "CC-e só pode ser emitida para NF-e autorizada.");

        var config = await _config.GetAsync(cancellationToken);
        if (config is null)
            return ResponseDefault<EmitirCCeCommandResult>.Conflict("Configuração fiscal ausente.");

        var xmlEvento = $"<eventoNFe><infEvento><tpEvento>110110</tpEvento>" +
                        $"<chNFe>{nfe.ChaveAcesso}</chNFe>" +
                        $"<nSeqEvento>{request.Sequencia}</nSeqEvento>" +
                        $"<xCorrecao>{System.Security.SecurityElement.Escape(request.Correcao)}</xCorrecao>" +
                        $"<xCondUso>A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970...</xCondUso>" +
                        $"</infEvento></eventoNFe>";

        var resultado = await _sefaz.EnviarEventoAsync(xmlEvento, nfe.Ambiente, config.Uf, cancellationToken);

        await _nfes.AddEventoAsync(new NFeEvento
        {
            TenantId = _tenantContext.TenantId,
            NFeId = nfe.Id,
            Tipo = TipoEventoNFe.CartaCorrecao,
            Sequencia = request.Sequencia,
            DataEvento = DateTime.UtcNow,
            Descricao = request.Correcao,
            ProtocoloAutorizacao = resultado.Protocolo,
            CodigoStatusSefaz = resultado.Codigo,
            MotivoSefaz = resultado.Motivo,
            CreatedBy = _tenantContext.UserId
        }, cancellationToken);

        if (!resultado.Sucesso)
            return ResponseDefault<EmitirCCeCommandResult>.Conflict(
                $"SEFAZ recusou CC-e: {resultado.Codigo} {resultado.Motivo}");

        return ResponseDefault<EmitirCCeCommandResult>.Ok(
            new EmitirCCeCommandResult(nfe.Id, request.Sequencia, resultado.Protocolo));
    }
}
