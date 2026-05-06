using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

public sealed class ObterContaReceberQueryHandler
    : IRequestHandler<ObterContaReceberQuery, ResponseDefault<ObterContaReceberQueryResult>>
{
    private readonly IContaReceberRepository _repo;

    public ObterContaReceberQueryHandler(IContaReceberRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterContaReceberQueryResult>> Handle(ObterContaReceberQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterContaReceberQueryResult>.NotFound("Conta a receber não encontrada.");

        return ResponseDefault<ObterContaReceberQueryResult>.Ok(new ObterContaReceberQueryResult(
            c.Id, c.Descricao, c.ClienteId, c.ReceitaId, c.PlanoDeContasId,
            c.ValorOriginal, c.ValorRecebido, c.Saldo,
            c.DataVencimento, c.DataRecebimento, c.Status,
            c.DiasAtrasoEm(DateTime.UtcNow.Date), c.ObservacaoRecebimento, c.CreatedAt));
    }
}
