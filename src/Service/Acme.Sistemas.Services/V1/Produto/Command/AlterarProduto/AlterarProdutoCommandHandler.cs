using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Produto.Command.AlterarProduto;

public sealed class AlterarProdutoCommandHandler
    : IRequestHandler<AlterarProdutoCommand, ResponseDefault<AlterarProdutoCommandResult>>
{
    private readonly IProdutoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarProdutoCommandHandler(IProdutoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarProdutoCommandResult>> Handle(AlterarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (produto is null)
            return ResponseDefault<AlterarProdutoCommandResult>.NotFound("Produto não encontrado.");

        if (!string.IsNullOrWhiteSpace(request.CodigoBarras)
            && !string.Equals(produto.CodigoBarras, request.CodigoBarras, StringComparison.Ordinal))
        {
            var existing = await _repo.GetByCodigoBarrasAsync(request.CodigoBarras, cancellationToken);
            if (existing is not null && existing.Id != produto.Id)
                return ResponseDefault<AlterarProdutoCommandResult>.Conflict(
                    $"Já existe outro produto com código de barras {request.CodigoBarras}.");
        }

        produto.Nome = request.Nome;
        produto.Descricao = request.Descricao;
        produto.CodigoBarras = request.CodigoBarras;
        produto.UnidadeMedida = request.UnidadeMedida;
        produto.TipoProdutoId = request.TipoProdutoId;
        produto.FornecedorId = request.FornecedorId;
        produto.CustoMedio = request.CustoMedio;
        produto.EstoqueMinimo = request.EstoqueMinimo;
        produto.Status = request.Status;
        produto.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(produto, cancellationToken);
        return ResponseDefault<AlterarProdutoCommandResult>.Ok(new AlterarProdutoCommandResult(produto.Id));
    }
}
