using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

public sealed record ExcluirUsuarioCommand(Guid Id) : IRequest<ResponseDefault>;
