namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.AtualizarInadimplenciaCliente;

public sealed record AtualizarInadimplenciaClienteRequest(
    bool Inadimplente,
    bool BloquearVendas);
