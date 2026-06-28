using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

public sealed class ListarStatusFechamentoQueryHandler
    : IRequestHandler<ListarStatusFechamentoQuery, ResponseDefault<ListarStatusFechamentoQueryResult>>
{
    private readonly IFechamentoPontoRepository _repo;

    public ListarStatusFechamentoQueryHandler(IFechamentoPontoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarStatusFechamentoQueryResult>> Handle(
        ListarStatusFechamentoQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.ListByCompetenciaAsync(request.Competencia, cancellationToken);
        var dto = items
            .Select(f => new ListarStatusFechamentoQueryItem(f.FuncionarioId, f.Status, f.FechadoEm))
            .ToList();
        return ResponseDefault<ListarStatusFechamentoQueryResult>.Ok(
            new ListarStatusFechamentoQueryResult(dto, dto.Count));
    }
}
