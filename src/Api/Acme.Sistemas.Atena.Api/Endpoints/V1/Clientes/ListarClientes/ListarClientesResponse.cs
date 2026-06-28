using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ListarClientes;

public sealed record ListarClientesResponseItem(
    Guid Id,
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? Email,
    string? Telefone,
    StatusAtivo Status,
    bool Inadimplente,
    bool BloqueadoVendas);

public sealed record ListarClientesResponse(
    IReadOnlyList<ListarClientesResponseItem> Items,
    long Total);
