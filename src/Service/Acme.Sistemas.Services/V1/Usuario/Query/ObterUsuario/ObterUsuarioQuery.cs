using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

public sealed record ObterUsuarioQuery(Guid Id) : IRequest<ResponseDefault<ObterUsuarioQueryResult>>;
