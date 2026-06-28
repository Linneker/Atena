using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

public sealed class ObterContaPagarQueryHandler
    : IRequestHandler<ObterContaPagarQuery, ResponseDefault<ObterContaPagarQueryResult>>
{
    private readonly IContaPagarRepository _repo;
    private readonly IFornecedorRepository _fornecedores;

    public ObterContaPagarQueryHandler(IContaPagarRepository repo, IFornecedorRepository fornecedores)
    {
        _repo = repo;
        _fornecedores = fornecedores;
    }

    public async Task<ResponseDefault<ObterContaPagarQueryResult>> Handle(ObterContaPagarQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterContaPagarQueryResult>.NotFound("Conta a pagar não encontrada.");

        string? fornecedorNome = null;
        if (c.FornecedorId.HasValue)
        {
            var nomes = await _fornecedores.GetNomesByIdsAsync(new[] { c.FornecedorId.Value }, cancellationToken);
            nomes.TryGetValue(c.FornecedorId.Value, out fornecedorNome);
        }

        return ResponseDefault<ObterContaPagarQueryResult>.Ok(new ObterContaPagarQueryResult(
            c.Id, c.Descricao,
            c.FornecedorId, fornecedorNome,
            c.DespesaId, c.PlanoDeContasId,
            c.ValorOriginal, c.ValorPago, c.Saldo,
            c.DataVencimento, c.DataPagamento, c.Status,
            c.Observacao, c.CreatedAt));
    }
}
