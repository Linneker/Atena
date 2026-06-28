namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.CriarCliente;

public sealed record CriarClienteResponse(
    Guid Id,
    string Nome,
    string Documento);
