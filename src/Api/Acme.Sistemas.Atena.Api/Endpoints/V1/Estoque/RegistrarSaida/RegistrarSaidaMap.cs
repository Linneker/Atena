using Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarSaida;

public static class RegistrarSaidaMap
{
    public static RegistrarSaidaCommand ToCommand(this RegistrarSaidaRequest request)
        => new(
            request.EstoqueId,
            request.ProdutoId,
            request.Quantidade,
            request.CustoUnitario,
            request.Origem,
            request.Motivo,
            request.ClienteId,
            request.DocumentoReferencia,
            request.DataMovimento);

    public static RegistrarSaidaResponse ToResponse(this RegistrarSaidaCommandResult result)
        => new(result.MovimentoId, result.NovoSaldoTotal, result.NovoSaldoDisponivel);
}
