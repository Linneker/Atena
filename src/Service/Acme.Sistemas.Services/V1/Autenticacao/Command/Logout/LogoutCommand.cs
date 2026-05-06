using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<ResponseDefault>;
