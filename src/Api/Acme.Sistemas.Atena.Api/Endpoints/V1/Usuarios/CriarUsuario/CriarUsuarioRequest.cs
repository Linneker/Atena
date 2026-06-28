namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.CriarUsuario;

public sealed record CriarUsuarioRequest(
    string NomeCompleto,
    string Email,
    string Senha);
