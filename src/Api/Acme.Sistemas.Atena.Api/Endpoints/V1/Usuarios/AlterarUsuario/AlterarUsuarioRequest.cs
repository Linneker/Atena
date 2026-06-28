using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.AlterarUsuario;

public sealed record AlterarUsuarioRequest(
    string NomeCompleto,
    string Email,
    StatusAtivo Status);
