using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

public sealed class ListarReceitasQueryHandler
    : IRequestHandler<ListarReceitasQuery, ResponseDefault<ListarReceitasQueryResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ICentroDeCustoRepository _centros;

    public ListarReceitasQueryHandler(IReceitaRepository receitas, ICentroDeCustoRepository centros)
    {
        _receitas = receitas;
        _centros = centros;
    }

    public async Task<ResponseDefault<ListarReceitasQueryResult>> Handle(
        ListarReceitasQuery request,
        CancellationToken cancellationToken)
    {
        var receitas = await _receitas.ListByFiltroAsync(
            request.Status, request.RecebimentoInicio, request.RecebimentoFim,
            request.Categoria, request.CompetenciaId,
            request.Skip, request.Take, cancellationToken);

        var total = await _receitas.CountByFiltroAsync(
            request.Status, request.RecebimentoInicio, request.RecebimentoFim,
            request.Categoria, request.CompetenciaId, cancellationToken);

        var centroIds = receitas
            .Where(r => r.CentroDeCustoId.HasValue)
            .Select(r => r.CentroDeCustoId!.Value);
        var nomesCentro = await _centros.GetNomesByIdsAsync(centroIds, cancellationToken);

        var items = receitas.Select(r => new ListarReceitasQueryItem(
            r.Id, r.Nome, r.Categoria, r.Valor, r.DataPrevistaRecebimento,
            r.StatusRecebimento, r.ValorRecebido, r.DataRecebimento,
            r.CompetenciaId,
            r.CentroDeCustoId,
            r.CentroDeCustoId.HasValue && nomesCentro.TryGetValue(r.CentroDeCustoId.Value, out var nome) ? nome : null,
            r.ClienteId, r.OrigemVendaId,
            r.ReceitaFixa)).ToList();

        return ResponseDefault<ListarReceitasQueryResult>.Ok(
            new ListarReceitasQueryResult(items, total, request.Skip, request.Take));
    }
}
