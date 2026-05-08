using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

public sealed record CriarOrcamentoCommand(
    Guid ClienteId,
    Guid? VendedorId,
    DateTime DataValidade,
    decimal? DescontoPercentual,
    string? Observacao,
    IReadOnlyList<OrcamentoItemDto> Itens) : IRequest<ResponseDefault<CriarOrcamentoCommandResult>>;

public sealed record CriarOrcamentoCommandResult(Guid Id, string Numero, decimal ValorTotal);
