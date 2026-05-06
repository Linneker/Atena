using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Produtos;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

public sealed class DefinirPrecoProdutoCommandHandler
    : IRequestHandler<DefinirPrecoProdutoCommand, ResponseDefault<DefinirPrecoProdutoCommandResult>>
{
    private readonly IProdutoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public DefinirPrecoProdutoCommandHandler(IProdutoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<DefinirPrecoProdutoCommandResult>> Handle(DefinirPrecoProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _repo.GetByIdAsync(request.ProdutoId, cancellationToken);
        if (produto is null)
            return ResponseDefault<DefinirPrecoProdutoCommandResult>.NotFound("Produto não encontrado.");

        var inicio = request.VigenciaInicio ?? DateTime.UtcNow;

        // Encerra preços vigentes do mesmo tipo
        await _repo.ExpirarPrecosAtuaisAsync(produto.Id, request.TipoValorProdutoId, inicio, cancellationToken);

        var preco = new ValorProduto
        {
            TenantId = _tenantContext.TenantId,
            ProdutoId = produto.Id,
            TipoValorProdutoId = request.TipoValorProdutoId,
            Valor = request.Valor,
            VigenciaInicio = inicio,
            CreatedBy = _tenantContext.UserId
        };
        await _repo.UpsertPrecoAsync(preco, cancellationToken);

        return ResponseDefault<DefinirPrecoProdutoCommandResult>.Created(
            new DefinirPrecoProdutoCommandResult(preco.Id, preco.Valor, preco.VigenciaInicio));
    }
}
