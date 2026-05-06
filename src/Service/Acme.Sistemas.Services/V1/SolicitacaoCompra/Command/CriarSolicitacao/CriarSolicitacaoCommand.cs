using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

public sealed record SolicitacaoItemDto(Guid ProdutoId, decimal Quantidade, decimal? PrecoEstimado, string? Observacao);

public sealed record CriarSolicitacaoCommand(
    string? Justificativa,
    IReadOnlyList<SolicitacaoItemDto> Itens,
    bool EnviarParaAprovacao = false) : IRequest<ResponseDefault<CriarSolicitacaoCommandResult>>;

public sealed record CriarSolicitacaoCommandResult(Guid Id, string Numero, decimal ValorTotal);
