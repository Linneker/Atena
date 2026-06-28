namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ConsultarSaldo;

public sealed record ConsultarSaldoResponseItem(
    Guid EstoqueId,
    decimal SaldoTotal,
    decimal SaldoReservado,
    decimal SaldoDisponivel);

public sealed record ConsultarSaldoResponse(
    Guid ProdutoId,
    decimal TotalGeral,
    decimal ReservadoGeral,
    decimal DisponivelGeral,
    IReadOnlyList<ConsultarSaldoResponseItem> PorEstoque);
