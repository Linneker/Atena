using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

public sealed record ObterFornecedorQuery(Guid Id) : IRequest<ResponseDefault<ObterFornecedorQueryResult>>;

public sealed record ObterFornecedorQueryResult(
    Guid Id, TipoPessoa Tipo, string Nome, string? NomeFantasia,
    string Documento, string? InscricaoEstadual,
    string? Email, string? Telefone,
    string? CondicaoPagamentoPadrao, StatusAtivo Status,
    Endereco Endereco, DateTime CreatedAt);
