using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ObterFornecedor;

public static class ObterFornecedorMap
{
    public static ObterFornecedorQuery ToQuery(this ObterFornecedorRequest request)
        => new(request.Id);

    public static ObterFornecedorResponse ToResponse(this ObterFornecedorQueryResult result)
        => new(result.Id, result.Tipo, result.Nome, result.NomeFantasia, result.Documento,
            result.InscricaoEstadual, result.Email, result.Telefone, result.CondicaoPagamentoPadrao,
            result.Status, result.Endereco.ToResponse(), result.CreatedAt);

    private static EnderecoResponse ToResponse(this Endereco endereco)
        => new(endereco.Cep, endereco.Logradouro, endereco.Numero, endereco.Complemento,
            endereco.Bairro, endereco.Cidade, endereco.Uf, endereco.Pais);
}
