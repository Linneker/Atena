using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;

public sealed record CriarTipoProdutoCommandResult(Guid Id, string Nome);
