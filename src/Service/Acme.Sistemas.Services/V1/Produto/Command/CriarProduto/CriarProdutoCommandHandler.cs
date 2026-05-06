using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using ProdutoEntity = Acme.Sistemas.Domain.Entities.Produtos.Produto;

namespace Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

public sealed class CriarProdutoCommandHandler
    : IRequestHandler<CriarProdutoCommand, ResponseDefault<CriarProdutoCommandResult>>
{
    private readonly IProdutoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarProdutoCommandHandler(IProdutoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarProdutoCommandResult>> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarProdutoCommandResult>.Conflict(
                $"Já existe produto com código {request.Codigo}.");

        if (!string.IsNullOrWhiteSpace(request.CodigoBarras))
        {
            var existingBarras = await _repo.GetByCodigoBarrasAsync(request.CodigoBarras, cancellationToken);
            if (existingBarras is not null)
                return ResponseDefault<CriarProdutoCommandResult>.Conflict(
                    $"Já existe produto com código de barras {request.CodigoBarras}.");
        }

        var produto = new ProdutoEntity
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Nome = request.Nome,
            Descricao = request.Descricao,
            CodigoBarras = request.CodigoBarras,
            UnidadeMedida = request.UnidadeMedida,
            TipoProdutoId = request.TipoProdutoId,
            FornecedorId = request.FornecedorId,
            CustoMedio = request.CustoMedio,
            EstoqueMinimo = request.EstoqueMinimo,
            Status = StatusAtivo.Ativo,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(produto, cancellationToken);
        return ResponseDefault<CriarProdutoCommandResult>.Created(
            new CriarProdutoCommandResult(produto.Id, produto.Codigo, produto.Nome));
    }
}
