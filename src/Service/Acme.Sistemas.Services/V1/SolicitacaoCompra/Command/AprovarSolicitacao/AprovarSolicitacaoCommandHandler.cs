using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Services;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.AprovarSolicitacao;

public sealed class AprovarSolicitacaoCommandHandler
    : IRequestHandler<AprovarSolicitacaoCommand, ResponseDefault<AprovarSolicitacaoCommandResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AprovarSolicitacaoCommandHandler(ISolicitacaoCompraRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AprovarSolicitacaoCommandResult>> Handle(AprovarSolicitacaoCommand request, CancellationToken cancellationToken)
    {
        var sol = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (sol is null)
            return ResponseDefault<AprovarSolicitacaoCommandResult>.NotFound("Solicitação não encontrada.");

        if (sol.Status != StatusSolicitacaoCompra.AguardandoAprovacao)
            return ResponseDefault<AprovarSolicitacaoCommandResult>.Conflict(
                $"Solicitação não está aguardando aprovação (status atual: {sol.Status}).");

        if (sol.SolicitanteId.HasValue && sol.SolicitanteId == _tenantContext.UserId)
            return ResponseDefault<AprovarSolicitacaoCommandResult>.Conflict(
                "Solicitante não pode aprovar a própria solicitação.");

        if (!AlcadaAprovacao.TemAlcada(_tenantContext.Permissions, sol.ValorTotal))
        {
            var necessaria = AlcadaAprovacao.PermissaoNecessaria(sol.ValorTotal);
            return ResponseDefault<AprovarSolicitacaoCommandResult>.Forbidden(
                $"Sem alçada para aprovar valor de {sol.ValorTotal:C}. Permissão necessária: {necessaria}.");
        }

        var now = DateTime.UtcNow;
        await _repo.UpdateStatusAsync(sol.Id, StatusSolicitacaoCompra.Aprovada, _tenantContext.UserId, now, null, cancellationToken);

        return ResponseDefault<AprovarSolicitacaoCommandResult>.Ok(
            new AprovarSolicitacaoCommandResult(sol.Id, now));
    }
}
