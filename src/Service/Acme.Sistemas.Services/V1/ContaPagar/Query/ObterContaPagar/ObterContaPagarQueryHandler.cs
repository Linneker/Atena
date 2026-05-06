using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

public sealed class ObterContaPagarQueryHandler
    : IRequestHandler<ObterContaPagarQuery, ResponseDefault<ObterContaPagarQueryResult>>
{
    private readonly IContaPagarRepository _repo;

    public ObterContaPagarQueryHandler(IContaPagarRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterContaPagarQueryResult>> Handle(ObterContaPagarQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterContaPagarQueryResult>.NotFound("Conta a pagar não encontrada.");

        return ResponseDefault<ObterContaPagarQueryResult>.Ok(new ObterContaPagarQueryResult(
            c.Id, c.Descricao, c.FornecedorId, c.DespesaId, c.PlanoDeContasId,
            c.ValorOriginal, c.ValorPago, c.Saldo,
            c.DataVencimento, c.DataPagamento, c.Status,
            c.Observacao, c.CreatedAt));
    }
}
