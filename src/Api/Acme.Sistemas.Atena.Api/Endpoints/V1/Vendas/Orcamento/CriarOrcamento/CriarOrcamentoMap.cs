using Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.CriarOrcamento;

public static class CriarOrcamentoMap
{
    public static CriarOrcamentoCommand ToCommand(this CriarOrcamentoRequest request)
        => new(
            request.ClienteId,
            request.VendedorId,
            request.DataValidade,
            request.DescontoPercentual,
            request.Observacao,
            request.Itens.Select(i => new OrcamentoItemDto(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToArray());

    public static CriarOrcamentoResponse ToResponse(this CriarOrcamentoCommandResult result)
        => new(result.Id, result.Numero, result.ValorTotal);
}
