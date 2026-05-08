using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

public sealed record CriarUsuarioCommand(
    string NomeCompleto,
    string Email,
    string Senha) : IRequest<ResponseDefault<CriarUsuarioCommandResult>>;

