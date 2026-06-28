using Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ObterCliente;

public static class ObterClienteMap
{
    public static ObterClienteQuery ToQuery(this ObterClienteRequest request)
        => new(request.Id);

    public static ObterClienteResponse ToResponse(this ObterClienteQueryResult result)
        => new(
            result.Id,
            result.Tipo,
            result.Nome,
            result.NomeFantasia,
            result.Documento,
            result.InscricaoEstadual,
            result.Email,
            result.Telefone,
            result.Status,
            result.Inadimplente,
            result.BloqueadoVendas,
            result.Endereco,
            result.CreatedAt);
}
