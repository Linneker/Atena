using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;
using Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.CriarFornecedor;

public static class CriarFornecedorMap
{
    public static CriarFornecedorCommand ToCommand(this CriarFornecedorRequest request)
        => new(request.Tipo, request.Nome, request.NomeFantasia, request.Documento,
            request.InscricaoEstadual, request.Email, request.Telefone,
            request.CondicaoPagamentoPadrao, request.Endereco?.ToDto());

    public static CriarFornecedorResponse ToResponse(this CriarFornecedorCommandResult result)
        => new(result.Id, result.Nome, result.Documento);

    private static EnderecoDto ToDto(this EnderecoRequest endereco)
        => new(endereco.Cep, endereco.Logradouro, endereco.Numero, endereco.Complemento,
            endereco.Bairro, endereco.Cidade, endereco.Uf);
}
