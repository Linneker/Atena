using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ListarMovimentos;

public sealed record ListarMovimentosResponseItem(
    Guid Id, DateOnly Data, OrigemMovimentoBancoHoras Origem, int Minutos, string? Observacao);

public sealed record ListarMovimentosResponse(
    IReadOnlyList<ListarMovimentosResponseItem> Items, int Total, int SaldoMinutos);
