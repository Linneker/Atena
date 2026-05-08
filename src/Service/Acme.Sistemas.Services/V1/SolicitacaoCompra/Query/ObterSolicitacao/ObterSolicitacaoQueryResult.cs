using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

public sealed record SolicitacaoItemView(Guid Id, Guid ProdutoId, decimal Quantidade, decimal? PrecoEstimado, string? Observacao);

public sealed record ObterSolicitacaoQueryResult(
    Guid Id, string Numero, Guid? SolicitanteId, string? Justificativa,
    decimal ValorTotal, DateTime DataSolicitacao,
    StatusSolicitacaoCompra Status, Guid? AprovadoPor, DateTime? AprovadoEm,
    string? MotivoRejeicao, IReadOnlyList<SolicitacaoItemView> Itens);
