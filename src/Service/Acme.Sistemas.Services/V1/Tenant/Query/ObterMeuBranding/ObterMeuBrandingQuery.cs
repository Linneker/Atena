using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

// Query sem parâmetros — o tenant é resolvido pelo ITenantContext do JWT.
public sealed record ObterMeuBrandingQuery() : IRequest<ResponseDefault<ObterMeuBrandingQueryResult>>;
