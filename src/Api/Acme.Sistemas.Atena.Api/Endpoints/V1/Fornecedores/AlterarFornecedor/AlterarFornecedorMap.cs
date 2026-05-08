using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;
using Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.AlterarFornecedor;

public static class AlterarFornecedorMap
{
    public static AlterarFornecedorCommand ToCommand(this AlterarFornecedorRequest request, Guid id)
        => new(id, request.Tipo, request.Nome, request.NomeFantasia, request.Documento,
            request.InscricaoEstadual, request.Email, request.Telefone,
            request.CondicaoPagamentoPadrao, request.Status, request.Endereco?.ToDto());

    public static AlterarFornecedorResponse ToResponse(this AlterarFornecedorCommandResult result)
        => new(result.Id);

    private static EnderecoDto ToDto(this EnderecoRequest endereco)
        => new(endereco.Cep, endereco.Logradouro, endereco.Numero, endereco.Complemento,
            endereco.Bairro, endereco.Cidade, endereco.Uf);
}
