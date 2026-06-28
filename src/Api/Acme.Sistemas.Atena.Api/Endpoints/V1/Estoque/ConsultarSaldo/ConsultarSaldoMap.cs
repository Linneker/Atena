using Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ConsultarSaldo;

public static class ConsultarSaldoMap
{
    public static ConsultarSaldoQuery ToQuery(this ConsultarSaldoRequest request)
        => new(request.ProdutoId, request.EstoqueId);

    public static ConsultarSaldoResponse ToResponse(this ConsultarSaldoQueryResult result)
        => new(
            result.ProdutoId,
            result.TotalGeral,
            result.ReservadoGeral,
            result.DisponivelGeral,
            result.PorEstoque.Select(p => p.ToResponseItem()).ToArray());

    private static ConsultarSaldoResponseItem ToResponseItem(this SaldoPorEstoque item)
        => new(item.EstoqueId, item.SaldoTotal, item.SaldoReservado, item.SaldoDisponivel);
}
