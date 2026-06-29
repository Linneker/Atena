using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

public sealed record ObterConfiguracaoRepQuery(Guid EmpresaId)
    : IRequest<ResponseDefault<ObterConfiguracaoRepQueryResult>>;
