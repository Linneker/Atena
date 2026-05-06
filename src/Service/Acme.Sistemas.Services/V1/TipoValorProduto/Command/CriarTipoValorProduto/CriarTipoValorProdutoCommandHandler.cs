using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using TipoValorProdutoEntity = Acme.Sistemas.Domain.Entities.Produtos.TipoValorProduto;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;

public sealed class CriarTipoValorProdutoCommandHandler
    : IRequestHandler<CriarTipoValorProdutoCommand, ResponseDefault<CriarTipoValorProdutoCommandResult>>
{
    private readonly ITipoValorProdutoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarTipoValorProdutoCommandHandler(ITipoValorProdutoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarTipoValorProdutoCommandResult>> Handle(CriarTipoValorProdutoCommand request, CancellationToken cancellationToken)
    {
        var tipo = new TipoValorProdutoEntity
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Ativo = true,
            CreatedBy = _tenantContext.UserId
        };
        await _repo.AddAsync(tipo, cancellationToken);
        return ResponseDefault<CriarTipoValorProdutoCommandResult>.Created(
            new CriarTipoValorProdutoCommandResult(tipo.Id, tipo.Nome));
    }
}
