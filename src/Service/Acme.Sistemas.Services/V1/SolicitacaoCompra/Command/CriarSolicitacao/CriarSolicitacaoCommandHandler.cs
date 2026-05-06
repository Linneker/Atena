using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Events;
using Acme.Sistemas.Services.V1.SolicitacaoCompra.Services;
using SolicitacaoEntity = Acme.Sistemas.Domain.Entities.Compras.SolicitacaoCompra;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

public sealed class CriarSolicitacaoCommandHandler
    : IRequestHandler<CriarSolicitacaoCommand, ResponseDefault<CriarSolicitacaoCommandResult>>
{
    private readonly ISolicitacaoCompraRepository _repo;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;

    public CriarSolicitacaoCommandHandler(
        ISolicitacaoCompraRepository repo, IMediator mediator, ITenantContext tenantContext)
    {
        _repo = repo;
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarSolicitacaoCommandResult>> Handle(CriarSolicitacaoCommand request, CancellationToken cancellationToken)
    {
        var seq = await _repo.NextNumeroAsync(cancellationToken);
        var numero = $"SC{DateTime.UtcNow:yyyyMM}{seq:D5}";

        var valorTotal = request.Itens.Sum(i => (i.PrecoEstimado ?? 0) * i.Quantidade);

        var solicitacao = new SolicitacaoEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            SolicitanteId = _tenantContext.UserId,
            Justificativa = request.Justificativa,
            ValorTotal = valorTotal,
            DataSolicitacao = DateTime.UtcNow,
            Status = request.EnviarParaAprovacao
                ? StatusSolicitacaoCompra.AguardandoAprovacao
                : StatusSolicitacaoCompra.Rascunho,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(solicitacao, cancellationToken);

        var itens = request.Itens.Select(i => new SolicitacaoCompraItem
        {
            TenantId = _tenantContext.TenantId,
            SolicitacaoCompraId = solicitacao.Id,
            ProdutoId = i.ProdutoId,
            Quantidade = i.Quantidade,
            PrecoEstimado = i.PrecoEstimado,
            Observacao = i.Observacao,
            CreatedBy = _tenantContext.UserId
        }).ToList();

        await _repo.AddItensAsync(itens, cancellationToken);

        if (solicitacao.Status == StatusSolicitacaoCompra.AguardandoAprovacao)
        {
            await _mediator.Publish(new NotificarAprovacaoPendenteNotification(
                _tenantContext.TenantId,
                solicitacao.Id,
                solicitacao.Numero,
                solicitacao.SolicitanteId,
                valorTotal,
                AlcadaAprovacao.PermissaoNecessaria(valorTotal),
                DateTime.UtcNow), cancellationToken);
        }

        return ResponseDefault<CriarSolicitacaoCommandResult>.Created(
            new CriarSolicitacaoCommandResult(solicitacao.Id, solicitacao.Numero, valorTotal));
    }
}
