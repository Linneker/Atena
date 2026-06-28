namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.AtualizarInadimplenciaCliente;

public sealed record AtualizarInadimplenciaClienteResponse(
    Guid Id,
    bool Inadimplente,
    bool BloqueadoVendas);
