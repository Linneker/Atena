using Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarEntrada;

public static class RegistrarEntradaMap
{
    public static RegistrarEntradaCommand ToCommand(this RegistrarEntradaRequest request)
        => new(
            request.EstoqueId,
            request.ProdutoId,
            request.Quantidade,
            request.CustoUnitario,
            request.Origem,
            request.Motivo,
            request.FornecedorId,
            request.DocumentoReferencia,
            request.DataMovimento);

    public static RegistrarEntradaResponse ToResponse(this RegistrarEntradaCommandResult result)
        => new(result.MovimentoId, result.NovoSaldoTotal, result.NovoSaldoDisponivel);
}
