using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CodigoServico.Query.ListarCodigosServico;

public sealed record ListarCodigosServicoQuery
    : IRequest<ResponseDefault<ListarCodigosServicoQueryResult>>;
