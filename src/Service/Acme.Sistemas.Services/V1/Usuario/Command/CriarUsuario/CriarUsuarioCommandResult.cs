using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

public sealed record CriarUsuarioCommandResult(Guid Id, string NomeCompleto, string Email);
