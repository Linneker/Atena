using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Query.ListarCbos;

public sealed record ListarCbosQuery() : IRequest<ResponseDefault<ListarCbosQueryResult>>;
