namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.ConfirmarEmail;

public sealed record ConfirmarEmailResponse(
    Guid UserId,
    string Email,
    DateTime ConfirmadoEm);
