using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ObterSolicitacao;

public sealed record ObterSolicitacaoResponseItem(
    Guid Id,
    Guid ProdutoId,
    decimal Quantidade,
    decimal? PrecoEstimado,
    string? Observacao);

public sealed record ObterSolicitacaoResponse(
    Guid Id,
    string Numero,
    Guid? SolicitanteId,
    string? Justificativa,
    decimal ValorTotal,
    DateTime DataSolicitacao,
    StatusSolicitacaoCompra Status,
    Guid? AprovadoPor,
    DateTime? AprovadoEm,
    string? MotivoRejeicao,
    IReadOnlyList<ObterSolicitacaoResponseItem> Itens);
