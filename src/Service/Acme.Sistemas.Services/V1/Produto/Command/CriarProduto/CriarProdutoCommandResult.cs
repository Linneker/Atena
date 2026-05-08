using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

public sealed record CriarProdutoCommandResult(Guid Id, string Codigo, string Nome);
