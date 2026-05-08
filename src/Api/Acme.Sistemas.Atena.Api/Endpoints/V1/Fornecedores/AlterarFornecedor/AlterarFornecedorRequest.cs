using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.AlterarFornecedor;

public sealed record EnderecoRequest(
    string? Cep,
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Uf);

public sealed record AlterarFornecedorRequest(
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    string? CondicaoPagamentoPadrao,
    StatusAtivo Status,
    EnderecoRequest? Endereco);
