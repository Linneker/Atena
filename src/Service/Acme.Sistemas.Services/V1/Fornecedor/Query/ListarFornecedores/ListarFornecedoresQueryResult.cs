using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

public sealed record ListarFornecedoresQueryItem(
    Guid Id, TipoPessoa Tipo, string Nome, string? NomeFantasia,
    string Documento, string? Email, string? Telefone,
    string? CondicaoPagamentoPadrao, StatusAtivo Status);

public sealed record ListarFornecedoresQueryResult(IReadOnlyList<ListarFornecedoresQueryItem> Items, long Total);
