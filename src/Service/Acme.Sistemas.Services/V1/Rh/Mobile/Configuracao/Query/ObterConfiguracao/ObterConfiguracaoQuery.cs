using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Configuracao.Query.ObterConfiguracao;

public sealed record ObterConfiguracaoQuery()
    : IRequest<ResponseDefault<ObterConfiguracaoQueryResult>>;
