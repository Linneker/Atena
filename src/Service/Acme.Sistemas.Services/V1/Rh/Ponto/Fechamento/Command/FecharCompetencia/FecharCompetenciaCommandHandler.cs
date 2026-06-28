using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using FechamentoEntity = Acme.Sistemas.Domain.Entities.Rh.FechamentoPonto;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.FecharCompetencia;

public sealed class FecharCompetenciaCommandHandler
    : IRequestHandler<FecharCompetenciaCommand, ResponseDefault<FecharCompetenciaCommandResult>>
{
    private readonly IFechamentoPontoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public FecharCompetenciaCommandHandler(IFechamentoPontoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<FecharCompetenciaCommandResult>> Handle(
        FecharCompetenciaCommand request, CancellationToken cancellationToken)
    {
        var existente = await _repo.GetByFuncionarioCompetenciaAsync(
            request.FuncionarioId, request.Competencia, cancellationToken);

        if (existente?.Status == StatusFechamentoPonto.Fechado)
            return ResponseDefault<FecharCompetenciaCommandResult>.Conflict(
                $"Competência {request.Competencia} já está fechada.");

        var userId = _tenantContext.UserId;
        if (existente is null)
        {
            existente = new FechamentoEntity
            {
                TenantId = _tenantContext.TenantId,
                FuncionarioId = request.FuncionarioId,
                Competencia = request.Competencia,
                Status = StatusFechamentoPonto.Fechado,
                FechadoEm = DateTime.UtcNow,
                FechadoPor = userId,
                Observacoes = request.Observacoes,
                CreatedBy = userId,
            };
            await _repo.AddAsync(existente, cancellationToken);
        }
        else
        {
            existente.Status = StatusFechamentoPonto.Fechado;
            existente.FechadoEm = DateTime.UtcNow;
            existente.FechadoPor = userId;
            existente.Observacoes = request.Observacoes ?? existente.Observacoes;
            existente.UpdatedBy = userId;
            await _repo.UpdateAsync(existente, cancellationToken);
        }

        return ResponseDefault<FecharCompetenciaCommandResult>.Ok(
            new FecharCompetenciaCommandResult(existente.Id, existente.Competencia));
    }
}
