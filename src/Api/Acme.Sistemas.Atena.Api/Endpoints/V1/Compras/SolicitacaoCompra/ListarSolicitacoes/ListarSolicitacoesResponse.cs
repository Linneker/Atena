using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ListarSolicitacoes;

public sealed record ListarSolicitacoesResponseItem(
    Guid Id,
    string Numero,
    Guid? SolicitanteId,
    decimal ValorTotal,
    DateTime DataSolicitacao,
    StatusSolicitacaoCompra Status);

public sealed record ListarSolicitacoesResponse(
    IReadOnlyList<ListarSolicitacoesResponseItem> Items,
    long Total);
