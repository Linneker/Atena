using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;

public sealed record ObterClienteQuery(Guid Id) : IRequest<ResponseDefault<ObterClienteQueryResult>>;

public sealed record ObterClienteQueryResult(
    Guid Id, TipoPessoa Tipo, string Nome, string? NomeFantasia,
    string Documento, string? InscricaoEstadual,
    string? Email, string? Telefone,
    StatusAtivo Status, bool Inadimplente, bool BloqueadoVendas,
    Endereco Endereco, DateTime CreatedAt);
