using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

public sealed class ObterReceitaQueryHandler
    : IRequestHandler<ObterReceitaQuery, ResponseDefault<ObterReceitaQueryResult>>
{
    private readonly IReceitaRepository _receitas;

    public ObterReceitaQueryHandler(IReceitaRepository receitas)
    {
        _receitas = receitas;
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

        return ResponseDefault<ObterReceitaQueryResult>.Ok(new ObterReceitaQueryResult(
            r.Id, r.Nome, r.Descricao, r.Categoria, r.Valor, r.ReceitaFixa,
            r.DataPrevistaRecebimento, r.CompetenciaId, r.CentroDeCustoId,
            r.ClienteId, r.OrigemVendaId, r.StatusRecebimento, r.ValorRecebido,
            r.DataRecebimento, r.FormaPagamento, r.ObservacaoRecebimento,
            r.CreatedAt, r.UpdatedAt));
    }
}
