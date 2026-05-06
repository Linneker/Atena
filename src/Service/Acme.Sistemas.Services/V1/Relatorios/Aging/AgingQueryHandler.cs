using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Relatorios.Aging;

public sealed class AgingQueryHandler
    : IRequestHandler<AgingQuery, ResponseDefault<AgingQueryResult>>
{
    private readonly IAgingRepository _repo;

    public AgingQueryHandler(IAgingRepository repo) => _repo = repo;

    public async Task<ResponseDefault<AgingQueryResult>> Handle(AgingQuery request, CancellationToken cancellationToken)
    {
        var raw = request.Tipo == TipoAging.ContasPagar
            ? await _repo.AgingContasPagarAsync(cancellationToken)
            : await _repo.AgingContasReceberAsync(cancellationToken);

        var faixas = raw.Select(x => new AgingFaixa(x.Faixa, x.Quantidade, x.Valor)).ToList();

        return ResponseDefault<AgingQueryResult>.Ok(new AgingQueryResult(
            request.Tipo,
            faixas,
            faixas.Sum(f => f.Valor),
            faixas.Sum(f => f.Quantidade)));
    }
}
