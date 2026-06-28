namespace Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

public sealed record ListarEstoquesQueryItem(
    Guid Id, string Codigo, string Nome, string? Localizacao, bool Ativo);

public sealed record ListarEstoquesQueryResult(
    IReadOnlyList<ListarEstoquesQueryItem> Items, long Total);
