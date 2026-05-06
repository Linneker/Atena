using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;

public sealed record ListarClientesQuery(
    string? Termo = null,
    bool? Inadimplente = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarClientesQueryResult>>;

public sealed record ListarClientesQueryItem(
    Guid Id, TipoPessoa Tipo, string Nome, string? NomeFantasia,
    string Documento, string? Email, string? Telefone,
    StatusAtivo Status, bool Inadimplente, bool BloqueadoVendas);

public sealed record ListarClientesQueryResult(IReadOnlyList<ListarClientesQueryItem> Items, long Total);
