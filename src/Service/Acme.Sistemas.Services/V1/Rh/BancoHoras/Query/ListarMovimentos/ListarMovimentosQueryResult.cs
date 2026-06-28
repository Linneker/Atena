using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

public sealed record ListarMovimentosQueryItem(
    Guid Id,
    DateOnly Data,
    OrigemMovimentoBancoHoras Origem,
    int Minutos,
    string? Observacao);

public sealed record ListarMovimentosQueryResult(
    IReadOnlyList<ListarMovimentosQueryItem> Items, int Total, int SaldoMinutos);
