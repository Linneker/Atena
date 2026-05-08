using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

public sealed record ObterDividaQuery(Guid Id) : IRequest<ResponseDefault<ObterDividaQueryResult>>;

