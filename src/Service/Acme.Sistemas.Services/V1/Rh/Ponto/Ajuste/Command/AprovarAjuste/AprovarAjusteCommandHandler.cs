using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Mobile.Push;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using MarcacaoEntity = Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

/// <summary>
/// Gestor aprova ajuste. Cria nova MarcacaoPonto (origem=Manual, status=Ajustada quando
/// substitui original) preservando hash-chain. Original mantém status=Ajustada via update lateral.
/// </summary>
public sealed class AprovarAjusteCommandHandler
    : IRequestHandler<AprovarAjusteCommand, ResponseDefault<AprovarAjusteCommandResult>>
{
    private readonly IAjustePontoRepository _ajusteRepo;
    private readonly IMarcacaoPontoRepository _marcacaoRepo;
    private readonly ITenantContext _tenantContext;
    private readonly INotificacaoPushService _push;

    public AprovarAjusteCommandHandler(
        IAjustePontoRepository ajusteRepo,
        IMarcacaoPontoRepository marcacaoRepo,
        ITenantContext tenantContext,
        INotificacaoPushService push)
    {
        _ajusteRepo = ajusteRepo;
        _marcacaoRepo = marcacaoRepo;
        _tenantContext = tenantContext;
        _push = push;
    }

    public async Task<ResponseDefault<AprovarAjusteCommandResult>> Handle(
        AprovarAjusteCommand request, CancellationToken cancellationToken)
    {
        var ajuste = await _ajusteRepo.GetByIdAsync(request.Id, cancellationToken);
        if (ajuste is null)
            return ResponseDefault<AprovarAjusteCommandResult>.NotFound($"Ajuste {request.Id} não encontrado.");

        if (ajuste.Status != StatusAjuste.Pendente)
            return ResponseDefault<AprovarAjusteCommandResult>.Conflict(
                $"Ajuste já foi decidido (status={ajuste.Status}).");

        var userId = _tenantContext.UserId;
        Guid? marcacaoResultanteId = null;

        // Para tipos que geram batida (Inclusao, AlteracaoHora), cria nova MarcacaoPonto
        if (ajuste.TipoAjuste is TipoAjuste.Inclusao or TipoAjuste.AlteracaoHora
            && ajuste.DataHoraProposta.HasValue)
        {
            var ultima = await _marcacaoRepo.GetUltimaPorFuncionarioAsync(ajuste.FuncionarioId, cancellationToken);
            var tipo = ajuste.TipoMarcacaoProposta ?? TipoMarcacao.Entrada;
            var origem = OrigemMarcacao.Manual;
            var hash = MarcacaoPontoIntegridade.Calcular(
                ajuste.FuncionarioId, ajuste.DataHoraProposta.Value, tipo, origem, ultima?.HashIntegridade);

            var nova = new MarcacaoEntity
            {
                TenantId = _tenantContext.TenantId,
                FuncionarioId = ajuste.FuncionarioId,
                Tipo = tipo,
                DataHora = ajuste.DataHoraProposta.Value,
                Origem = origem,
                HashAnterior = ultima?.HashIntegridade,
                HashIntegridade = hash,
                Status = ajuste.TipoAjuste == TipoAjuste.AlteracaoHora
                    ? StatusMarcacao.Ajustada : StatusMarcacao.Valida,
                MarcacaoOrigemId = ajuste.MarcacaoOriginalId,
                CreatedBy = userId,
            };
            await _marcacaoRepo.AddAsync(nova, cancellationToken);
            marcacaoResultanteId = nova.Id;
        }

        ajuste.Status = StatusAjuste.Aprovado;
        ajuste.AprovadorId = userId;
        ajuste.DecisaoEm = DateTime.UtcNow;
        ajuste.JustificativaDecisao = request.Justificativa;
        ajuste.MarcacaoResultanteId = marcacaoResultanteId;
        ajuste.UpdatedBy = userId;
        await _ajusteRepo.UpdateAsync(ajuste, cancellationToken);

        await _push.EnviarParaTopicoAsync(
            $"funcionario:{ajuste.FuncionarioId}",
            "Ajuste aprovado",
            "Sua solicitação de ajuste de ponto foi aprovada.",
            new Dictionary<string, string>
            {
                ["ajusteId"] = ajuste.Id.ToString(),
                ["tipo"] = "ajuste-aprovado",
            },
            cancellationToken);

        return ResponseDefault<AprovarAjusteCommandResult>.Ok(
            new AprovarAjusteCommandResult(ajuste.Id, marcacaoResultanteId));
    }
}
