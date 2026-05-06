using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Services;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;

public sealed class RejeitarSolicitacaoCommandHandler
    : IRequestHandler<RejeitarSolicitacaoCommand, ResponseDefault<RejeitarSolicitacaoCommandResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;
    private readonly ITenantContext _tenantContext;

    public RejeitarSolicitacaoCommandHandler(ISolicitacaoCompraRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RejeitarSolicitacaoCommandResult>> Handle(RejeitarSolicitacaoCommand request, CancellationToken cancellationToken)
    {
        var sol = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (sol is null)
            return ResponseDefault<RejeitarSolicitacaoCommandResult>.NotFound("Solicitação não encontrada.");

        if (sol.Status != StatusSolicitacaoCompra.AguardandoAprovacao)
            return ResponseDefault<RejeitarSolicitacaoCommandResult>.Conflict(
                $"Solicitação não está aguardando aprovação (status atual: {sol.Status}).");

        // Mesma alçada da aprovação: para rejeitar é necessária autoridade equivalente
        if (!AlcadaAprovacao.TemAlcada(_tenantContext.Permissions, sol.ValorTotal))
        {
            var necessaria = AlcadaAprovacao.PermissaoNecessaria(sol.ValorTotal);
            return ResponseDefault<RejeitarSolicitacaoCommandResult>.Forbidden(
                $"Sem alçada para rejeitar valor de {sol.ValorTotal:C}. Permissão necessária: {necessaria}.");
        }

        var now = DateTime.UtcNow;
        await _repo.UpdateStatusAsync(sol.Id, StatusSolicitacaoCompra.Rejeitada, _tenantContext.UserId, now, request.Motivo, cancellationToken);

        return ResponseDefault<RejeitarSolicitacaoCommandResult>.Ok(
            new RejeitarSolicitacaoCommandResult(sol.Id, now));
    }
}
