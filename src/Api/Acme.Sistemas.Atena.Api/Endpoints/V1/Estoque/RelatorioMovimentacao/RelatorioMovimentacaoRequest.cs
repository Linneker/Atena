namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RelatorioMovimentacao;

public sealed record RelatorioMovimentacaoRequest(
    Guid ProdutoId,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 200);
