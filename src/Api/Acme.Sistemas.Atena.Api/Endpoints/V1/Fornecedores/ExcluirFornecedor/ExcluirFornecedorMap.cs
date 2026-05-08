using Acme.Sistemas.Services.V1.Fornecedor.Command.ExcluirFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fornecedores.ExcluirFornecedor;

public static class ExcluirFornecedorMap
{
    public static ExcluirFornecedorCommand ToCommand(this ExcluirFornecedorRequest request)
        => new(request.Id);
}
