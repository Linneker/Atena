using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

public sealed record ListarCstsQuery(string Tipo)
    : IRequest<ResponseDefault<ListarCstsQueryResult>>;
