using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;

public sealed record CriarTipoValorProdutoCommandResult(Guid Id, string Nome);
