using Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ListarClientes;

public static class ListarClientesMap
{
    public static ListarClientesQuery ToQuery(this ListarClientesRequest request)
        => new(request.Termo, request.Inadimplente, request.Skip, request.Take);

    public static ListarClientesResponse ToResponse(this ListarClientesQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total);

    private static ListarClientesResponseItem ToResponseItem(this ListarClientesQueryItem item)
        => new(
            item.Id,
            item.Tipo,
            item.Nome,
            item.NomeFantasia,
            item.Documento,
            item.Email,
            item.Telefone,
            item.Status,
            item.Inadimplente,
            item.BloqueadoVendas);
}
