using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

public sealed class ObterContaReceberQueryHandler
    : IRequestHandler<ObterContaReceberQuery, ResponseDefault<ObterContaReceberQueryResult>>
{
    private readonly IContaReceberRepository _repo;
    private readonly IClienteRepository _clientes;

    public ObterContaReceberQueryHandler(IContaReceberRepository repo, IClienteRepository clientes)
    {
        _repo = repo;
        _clientes = clientes;
    }

    public async Task<ResponseDefault<ObterContaReceberQueryResult>> Handle(ObterContaReceberQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterContaReceberQueryResult>.NotFound("Conta a receber não encontrada.");

        string? clienteNome = null;
        if (c.ClienteId.HasValue)
        {
            var nomes = await _clientes.GetNomesByIdsAsync(new[] { c.ClienteId.Value }, cancellationToken);
            nomes.TryGetValue(c.ClienteId.Value, out clienteNome);
        }

        return ResponseDefault<ObterContaReceberQueryResult>.Ok(new ObterContaReceberQueryResult(
            c.Id, c.Descricao,
            c.ClienteId, clienteNome,
            c.ReceitaId, c.PlanoDeContasId,
            c.ValorOriginal, c.ValorRecebido, c.Saldo,
            c.DataVencimento, c.DataRecebimento, c.Status,
            c.DiasAtrasoEm(DateTime.UtcNow.Date), c.ObservacaoRecebimento, c.CreatedAt));
    }
}
