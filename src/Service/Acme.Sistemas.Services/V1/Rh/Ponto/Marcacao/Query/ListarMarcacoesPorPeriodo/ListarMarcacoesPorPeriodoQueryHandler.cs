using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

public sealed class ListarMarcacoesPorPeriodoQueryHandler
    : IRequestHandler<ListarMarcacoesPorPeriodoQuery, ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>>
{
    private readonly IMarcacaoPontoRepository _repo;

    public ListarMarcacoesPorPeriodoQueryHandler(IMarcacaoPontoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>> Handle(
        ListarMarcacoesPorPeriodoQuery request, CancellationToken cancellationToken)
    {
        var marcacoes = await _repo.ListByFuncionarioPeriodoAsync(
            request.FuncionarioId, request.DataInicio, request.DataFim, cancellationToken);

        var items = marcacoes
            .Select(m => new ListarMarcacoesPorPeriodoQueryItem(
                m.Id, m.DataHora, m.Tipo, m.Origem, m.Status, m.HashIntegridade))
            .ToList();

        return ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>.Ok(
            new ListarMarcacoesPorPeriodoQueryResult(items, items.Count));
    }
}
