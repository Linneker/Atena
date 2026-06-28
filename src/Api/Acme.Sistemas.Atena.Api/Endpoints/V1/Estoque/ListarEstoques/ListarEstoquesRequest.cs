namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ListarEstoques;

public sealed record ListarEstoquesRequest(int Skip = 0, int Take = 100);
