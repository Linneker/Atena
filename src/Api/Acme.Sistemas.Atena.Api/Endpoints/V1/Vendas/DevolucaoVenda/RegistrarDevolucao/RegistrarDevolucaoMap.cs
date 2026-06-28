using Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.DevolucaoVenda.RegistrarDevolucao;

public static class RegistrarDevolucaoMap
{
    public static RegistrarDevolucaoCommand ToCommand(this RegistrarDevolucaoRequest request)
        => new(
            request.FaturamentoId,
            request.EstoqueDestinoId,
            request.Motivo,
            request.Itens.Select(i => new DevolucaoItemDto(i.FaturamentoItemId, i.Quantidade)).ToArray());

    public static RegistrarDevolucaoResponse ToResponse(this RegistrarDevolucaoCommandResult result)
        => new(result.DevolucaoId, result.ValorDevolvido, result.ContaReceberEstornada, result.NFeDevolucaoSolicitada);
}
