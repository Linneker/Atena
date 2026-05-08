using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.CriarFornecedor;

public sealed record EnderecoRequest(
    string? Cep,
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Uf);

public sealed record CriarFornecedorRequest(
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    string? CondicaoPagamentoPadrao,
    EnderecoRequest? Endereco);
