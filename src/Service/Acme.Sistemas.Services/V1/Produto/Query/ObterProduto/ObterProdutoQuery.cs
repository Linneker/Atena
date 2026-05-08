using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

public sealed record ObterProdutoQuery(Guid Id) : IRequest<ResponseDefault<ObterProdutoQueryResult>>;

