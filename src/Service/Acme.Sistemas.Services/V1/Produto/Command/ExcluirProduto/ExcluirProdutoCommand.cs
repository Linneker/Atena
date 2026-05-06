using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;

public sealed record ExcluirProdutoCommand(Guid Id) : IRequest<ResponseDefault>;
