using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

public sealed record SolicitacaoItemDto(Guid ProdutoId, decimal Quantidade, decimal? PrecoEstimado, string? Observacao);

