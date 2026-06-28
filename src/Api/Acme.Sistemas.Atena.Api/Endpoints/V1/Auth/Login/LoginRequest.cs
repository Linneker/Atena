namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Login;

public sealed record LoginRequest(
    string Email,
    string Senha);
