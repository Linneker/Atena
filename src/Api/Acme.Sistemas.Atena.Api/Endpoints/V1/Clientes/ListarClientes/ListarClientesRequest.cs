namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ListarClientes;

public sealed record ListarClientesRequest(
    string? Termo = null,
    bool? Inadimplente = null,
    int Skip = 0,
    int Take = 50);
