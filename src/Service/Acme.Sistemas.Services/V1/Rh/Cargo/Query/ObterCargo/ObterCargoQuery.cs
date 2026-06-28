using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

public sealed record ObterCargoQuery(Guid Id) : IRequest<ResponseDefault<ObterCargoQueryResult>>;
