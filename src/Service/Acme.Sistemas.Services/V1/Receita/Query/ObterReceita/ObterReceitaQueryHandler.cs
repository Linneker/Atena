using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

public sealed class ObterReceitaQueryHandler
    : IRequestHandler<ObterReceitaQuery, ResponseDefault<ObterReceitaQueryResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ICentroDeCustoRepository _centros;

    public ObterReceitaQueryHandler(IReceitaRepository receitas, ICentroDeCustoRepository centros)
    {
        _receitas = receitas;
        _centros = centros;
    }

    public async Task<ResponseDefault<ObterReceitaQueryResult>> Handle(
        ObterReceitaQuery request,
        CancellationToken cancellationToken)
    {
        var r = await _receitas.GetByIdAsync(request.Id, cancellationToken);
        if (r is null)
        {
            return ResponseDefault<ObterReceitaQueryResult>.NotFound("Receita não encontrada.");
        }

        string? centroNome = null;
        if (r.CentroDeCustoId.HasValue)
        {
            var nomes = await _centros.GetNomesByIdsAsync(new[] { r.CentroDeCustoId.Value }, cancellationToken);
            nomes.TryGetValue(r.CentroDeCustoId.Value, out centroNome);
        }

        return ResponseDefault<ObterReceitaQueryResult>.Ok(new ObterReceitaQueryResult(
            r.Id, r.Nome, r.Descricao, r.Categoria, r.Valor, r.ReceitaFixa,
            r.DataPrevistaRecebimento, r.CompetenciaId, r.CentroDeCustoId, centroNome,
            r.ClienteId, r.OrigemVendaId, r.StatusRecebimento, r.ValorRecebido,
            r.DataRecebimento, r.FormaPagamento, r.ObservacaoRecebimento,
            r.CreatedAt, r.UpdatedAt));
    }
}
