using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

public sealed record ObterComprovantePdfQuery(Guid MarcacaoId)
    : IRequest<ResponseDefault<ObterComprovantePdfQueryResult>>;
