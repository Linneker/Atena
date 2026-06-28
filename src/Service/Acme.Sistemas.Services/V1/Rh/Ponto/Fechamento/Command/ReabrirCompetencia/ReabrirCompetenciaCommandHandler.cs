using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.ReabrirCompetencia;

public sealed class ReabrirCompetenciaCommandHandler
    : IRequestHandler<ReabrirCompetenciaCommand, ResponseDefault<ReabrirCompetenciaCommandResult>>
{
    private readonly IFechamentoPontoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public ReabrirCompetenciaCommandHandler(IFechamentoPontoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ReabrirCompetenciaCommandResult>> Handle(
        ReabrirCompetenciaCommand request, CancellationToken cancellationToken)
    {
        var fechamento = await _repo.GetByFuncionarioCompetenciaAsync(
            request.FuncionarioId, request.Competencia, cancellationToken);

        if (fechamento is null)
            return ResponseDefault<ReabrirCompetenciaCommandResult>.NotFound(
                $"Sem fechamento para {request.FuncionarioId} em {request.Competencia}.");

        if (fechamento.Status != StatusFechamentoPonto.Fechado)
            return ResponseDefault<ReabrirCompetenciaCommandResult>.Conflict(
                $"Status atual ({fechamento.Status}) não permite reabertura.");

        fechamento.Status = StatusFechamentoPonto.Reaberto;
        fechamento.ReabertoEm = DateTime.UtcNow;
        fechamento.ReabertoPor = _tenantContext.UserId;
        fechamento.MotivoReabertura = request.Motivo;
        fechamento.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(fechamento, cancellationToken);
        return ResponseDefault<ReabrirCompetenciaCommandResult>.Ok(
            new ReabrirCompetenciaCommandResult(fechamento.Id));
    }
}
