using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

public sealed record DevolucaoItemDto(Guid FaturamentoItemId, decimal Quantidade);

public sealed record RegistrarDevolucaoCommand(
    Guid FaturamentoId,
    Guid EstoqueDestinoId,
    string? Motivo,
    IReadOnlyList<DevolucaoItemDto> Itens) : IRequest<ResponseDefault<RegistrarDevolucaoCommandResult>>;

public sealed record RegistrarDevolucaoCommandResult(
    Guid DevolucaoId, decimal ValorDevolvido, bool ContaReceberEstornada, bool NFeDevolucaoSolicitada);
