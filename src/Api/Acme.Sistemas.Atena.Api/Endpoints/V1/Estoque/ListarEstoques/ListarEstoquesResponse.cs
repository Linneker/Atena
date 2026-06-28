namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ListarEstoques;

public sealed record ListarEstoquesResponseItem(
    Guid Id, string Codigo, string Nome, string? Localizacao, bool Ativo);

public sealed record ListarEstoquesResponse(
    IReadOnlyList<ListarEstoquesResponseItem> Items, long Total);
