using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.RejeitarAjuste;

public sealed class RejeitarAjusteCommandHandler
    : IRequestHandler<RejeitarAjusteCommand, ResponseDefault<RejeitarAjusteCommandResult>>
{
    private readonly IAjustePontoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public RejeitarAjusteCommandHandler(IAjustePontoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RejeitarAjusteCommandResult>> Handle(
        RejeitarAjusteCommand request, CancellationToken cancellationToken)
    {
        var ajuste = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (ajuste is null)
            return ResponseDefault<RejeitarAjusteCommandResult>.NotFound($"Ajuste {request.Id} não encontrado.");

        if (ajuste.Status != StatusAjuste.Pendente)
            return ResponseDefault<RejeitarAjusteCommandResult>.Conflict(
                $"Ajuste já foi decidido (status={ajuste.Status}).");

        ajuste.Status = StatusAjuste.Rejeitado;
        ajuste.AprovadorId = _tenantContext.UserId;
        ajuste.DecisaoEm = DateTime.UtcNow;
        ajuste.JustificativaDecisao = request.Justificativa;
        ajuste.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(ajuste, cancellationToken);
        return ResponseDefault<RejeitarAjusteCommandResult>.Ok(new RejeitarAjusteCommandResult(ajuste.Id));
    }
}
