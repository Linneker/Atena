namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ListarFornecedores;

public sealed record ListarFornecedoresRequest(string? Termo = null, int? Skip = null, int? Take = null);
