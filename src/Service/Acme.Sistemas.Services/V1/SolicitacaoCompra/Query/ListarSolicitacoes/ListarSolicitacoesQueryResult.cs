using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

public sealed record ListarSolicitacoesQueryItem(
    Guid Id, string Numero, Guid? SolicitanteId,
    decimal ValorTotal, DateTime DataSolicitacao, StatusSolicitacaoCompra Status);

public sealed record ListarSolicitacoesQueryResult(IReadOnlyList<ListarSolicitacoesQueryItem> Items, long Total);
