using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ListarFornecedores;

public sealed record ListarFornecedoresResponseItem(
    Guid Id,
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? Email,
    string? Telefone,
    string? CondicaoPagamentoPadrao,
    StatusAtivo Status);

public sealed record ListarFornecedoresResponse(IReadOnlyList<ListarFornecedoresResponseItem> Items, long Total);
