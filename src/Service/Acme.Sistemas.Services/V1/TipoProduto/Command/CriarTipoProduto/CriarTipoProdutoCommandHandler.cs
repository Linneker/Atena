using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using TipoProdutoEntity = Acme.Sistemas.Domain.Entities.Produtos.TipoProduto;

namespace Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;

public sealed class CriarTipoProdutoCommandHandler
    : IRequestHandler<CriarTipoProdutoCommand, ResponseDefault<CriarTipoProdutoCommandResult>>
{
    private readonly ITipoProdutoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarTipoProdutoCommandHandler(ITipoProdutoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarTipoProdutoCommandResult>> Handle(CriarTipoProdutoCommand request, CancellationToken cancellationToken)
    {
        var tipo = new TipoProdutoEntity
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Ativo = true,
            CreatedBy = _tenantContext.UserId
        };
        await _repo.AddAsync(tipo, cancellationToken);
        return ResponseDefault<CriarTipoProdutoCommandResult>.Created(
            new CriarTipoProdutoCommandResult(tipo.Id, tipo.Nome));
    }
}
