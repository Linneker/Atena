namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.LoginMobile;

public sealed record LoginMobileRequest(string Email, string Senha, string DeviceId, string Plataforma);
