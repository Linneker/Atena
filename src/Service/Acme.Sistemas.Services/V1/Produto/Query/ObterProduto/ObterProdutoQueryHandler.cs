using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

public sealed class ObterProdutoQueryHandler
    : IRequestHandler<ObterProdutoQuery, ResponseDefault<ObterProdutoQueryResult>>
{
    private readonly IProdutoRepository _repo;
    private readonly IFornecedorRepository _fornecedores;

    public ObterProdutoQueryHandler(IProdutoRepository repo, IFornecedorRepository fornecedores)
    {
        _repo = repo;
        _fornecedores = fornecedores;
    }

    public async Task<ResponseDefault<ObterProdutoQueryResult>> Handle(ObterProdutoQuery request, CancellationToken cancellationToken)
    {
        var p = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (p is null)
            return ResponseDefault<ObterProdutoQueryResult>.NotFound("Produto não encontrado.");

        var precos = await _repo.ListPrecosAsync(p.Id, cancellationToken);
        var precosResult = precos
            .Select(x => new PrecoVigente(x.Id, x.TipoValorProdutoId, x.Valor, x.VigenciaInicio, x.VigenciaFim))
            .ToList();

        string? fornecedorNome = null;
        if (p.FornecedorId.HasValue)
        {
            var nomes = await _fornecedores.GetNomesByIdsAsync(new[] { p.FornecedorId.Value }, cancellationToken);
            nomes.TryGetValue(p.FornecedorId.Value, out fornecedorNome);
        }

        return ResponseDefault<ObterProdutoQueryResult>.Ok(new ObterProdutoQueryResult(
            p.Id, p.Codigo, p.Nome, p.Descricao,
            p.CodigoBarras, p.UnidadeMedida,
            p.TipoProdutoId,
            p.FornecedorId, fornecedorNome,
            p.CustoMedio, p.EstoqueMinimo,
            p.Status, precosResult));
    }
}
