using Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ListarMovimentos;

public static class ListarMovimentosMap
{
    public static ListarMovimentosQuery ToQuery(this ListarMovimentosRequest r)
        => new(r.FuncionarioId, r.Competencia);

    public static ListarMovimentosResponse ToResponse(this ListarMovimentosQueryResult r)
        => new(
            r.Items.Select(i => new ListarMovimentosResponseItem(
                i.Id, i.Data, i.Origem, i.Minutos, i.Observacao)).ToList(),
            r.Total, r.SaldoMinutos);
}
