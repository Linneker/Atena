using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ObterFornecedor;

public sealed record EnderecoResponse(
    string? Cep,
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Uf,
    string? Pais);

public sealed record ObterFornecedorResponse(
    Guid Id,
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    string? CondicaoPagamentoPadrao,
    StatusAtivo Status,
    EnderecoResponse Endereco,
    DateTime CreatedAt);
