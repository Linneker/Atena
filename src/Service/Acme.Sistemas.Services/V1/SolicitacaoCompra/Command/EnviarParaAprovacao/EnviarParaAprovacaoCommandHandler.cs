using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Events;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Services;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;

public sealed class EnviarParaAprovacaoCommandHandler
    : IRequestHandler<EnviarParaAprovacaoCommand, ResponseDefault<EnviarParaAprovacaoCommandResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;

    public EnviarParaAprovacaoCommandHandler(
        ISolicitacaoCompraRepository repo,
        IMediator mediator,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<EnviarParaAprovacaoCommandResult>> Handle(EnviarParaAprovacaoCommand request, CancellationToken cancellationToken)
    {
        var sol = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (sol is null)
            return ResponseDefault<EnviarParaAprovacaoCommandResult>.NotFound("Solicitação não encontrada.");

        if (sol.Status != StatusSolicitacaoCompra.Rascunho)
            return ResponseDefault<EnviarParaAprovacaoCommandResult>.Conflict(
                $"Apenas solicitações em Rascunho podem ser enviadas para aprovação (status atual: {sol.Status}).");

        await _repo.UpdateStatusOnlyAsync(sol.Id, StatusSolicitacaoCompra.AguardandoAprovacao, cancellationToken);

        var permissao = AlcadaAprovacao.PermissaoNecessaria(sol.ValorTotal);
        await _mediator.Publish(new NotificarAprovacaoPendenteNotification(
            _tenantContext.TenantId, sol.Id, sol.Numero, sol.SolicitanteId,
            sol.ValorTotal, permissao, DateTime.UtcNow), cancellationToken);

        return ResponseDefault<EnviarParaAprovacaoCommandResult>.Ok(
            new EnviarParaAprovacaoCommandResult(sol.Id, sol.ValorTotal, permissao));
    }
}
