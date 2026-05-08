using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Fiscal;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.CancelarNFe;

public sealed class CancelarNFeCommandHandler
    : IRequestHandler<CancelarNFeCommand, ResponseDefault<CancelarNFeCommandResult>>
{
    private static readonly TimeSpan PrazoCancelamento = TimeSpan.FromHours(24);

    private readonly INFeRepository _nfes;
    private readonly IConfiguracaoFiscalRepository _config;
    private readonly INFeSefazClient _sefaz;
    private readonly IFaturamentoRepository _faturamentos;
    private readonly IContaReceberRepository _contasReceber;
    private readonly ITenantContext _tenantContext;

    public CancelarNFeCommandHandler(
        INFeRepository nfes,
        IConfiguracaoFiscalRepository config,
        INFeSefazClient sefaz,
        IFaturamentoRepository faturamentos,
        IContaReceberRepository contasReceber,
        ITenantContext tenantContext)
    {
        _nfes = nfes;
        _config = config;
        _sefaz = sefaz;
        _faturamentos = faturamentos;
        _contasReceber = contasReceber;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CancelarNFeCommandResult>> Handle(CancelarNFeCommand request, CancellationToken cancellationToken)
    {
        var nfe = await _nfes.GetByIdAsync(request.NFeId, cancellationToken);
        if (nfe is null)
            return ResponseDefault<CancelarNFeCommandResult>.NotFound("NF-e não encontrada.");

        if (nfe.Status != StatusNFe.Autorizada)
            return ResponseDefault<CancelarNFeCommandResult>.Conflict(
                $"Apenas NF-e autorizada pode ser cancelada (status atual: {nfe.Status}).");

        if (!nfe.DataAutorizacao.HasValue || DateTime.UtcNow - nfe.DataAutorizacao.Value > PrazoCancelamento)
            return ResponseDefault<CancelarNFeCommandResult>.Conflict(
                $"Prazo de cancelamento (24h) expirado. Para inutilizar/substituir, emita NF-e de devolução.");

        var config = await _config.GetAsync(cancellationToken);
        if (config is null)
            return ResponseDefault<CancelarNFeCommandResult>.Conflict("Configuração fiscal ausente.");

        // Stub: monta e transmite evento
        var xmlEvento = $"<eventoNFe><infEvento><tpEvento>110111</tpEvento>" +
                        $"<chNFe>{nfe.ChaveAcesso}</chNFe><nProt>{nfe.ProtocoloAutorizacao}</nProt>" +
                        $"<xJust>{System.Security.SecurityElement.Escape(request.Justificativa)}</xJust></infEvento></eventoNFe>";

        var resultado = await _sefaz.EnviarEventoAsync(xmlEvento, nfe.Ambiente, config.Uf, cancellationToken);
        if (!resultado.Sucesso)
        {
            await _nfes.AddEventoAsync(new NFeEvento
            {
                TenantId = _tenantContext.TenantId,
                NFeId = nfe.Id,
                Tipo = TipoEventoNFe.Cancelamento,
                DataEvento = DateTime.UtcNow,
                Descricao = request.Justificativa,
                CodigoStatusSefaz = resultado.Codigo,
                MotivoSefaz = resultado.Motivo,
                CreatedBy = _tenantContext.UserId
            }, cancellationToken);
            return ResponseDefault<CancelarNFeCommandResult>.Conflict(
                $"SEFAZ recusou cancelamento: {resultado.Codigo} {resultado.Motivo}");
        }

        await _nfes.AddEventoAsync(new NFeEvento
        {
            TenantId = _tenantContext.TenantId,
            NFeId = nfe.Id,
            Tipo = TipoEventoNFe.Cancelamento,
            DataEvento = DateTime.UtcNow,
            Descricao = request.Justificativa,
            ProtocoloAutorizacao = resultado.Protocolo,
            CodigoStatusSefaz = resultado.Codigo,
            MotivoSefaz = resultado.Motivo,
            CreatedBy = _tenantContext.UserId
        }, cancellationToken);

        await _nfes.UpdateStatusAsync(nfe.Id, StatusNFe.Cancelada,
            resultado.Codigo, resultado.Motivo, resultado.Protocolo, null, null, null, cancellationToken);

        // Reverte conta a receber e (idealmente) estoque via faturamento vinculado
        bool contaCancelada = false;
        if (nfe.FaturamentoId.HasValue)
        {
            var fat = await _faturamentos.GetByIdAsync(nfe.FaturamentoId.Value, cancellationToken);
            if (fat?.ContaReceberId is Guid cid)
            {
                var cr = await _contasReceber.GetByIdAsync(cid, cancellationToken);
                if (cr is not null && cr.Status == StatusConta.Pendente)
                {
                    cr.Status = StatusConta.Cancelado;
                    cr.UpdatedBy = _tenantContext.UserId;
                    await _contasReceber.UpdateAsync(cr, cancellationToken);
                    contaCancelada = true;
                }
            }
        }

        // Estoque: a reversão completa exige cruzar faturamento_itens com produtos e gerar entradas.
        // Aqui apenas marcamos o flag — implementação completa fica para próxima rodada de hardening.
        const bool estoqueRevertido = false;

        return ResponseDefault<CancelarNFeCommandResult>.Ok(
            new CancelarNFeCommandResult(nfe.Id, resultado.Protocolo, estoqueRevertido, contaCancelada));
    }
}
