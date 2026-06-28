namespace Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

/// <summary>
/// <see cref="SenhaInicial"/> só é preenchida quando <see cref="EhNovo"/> é true (criação).
/// Em chamada idempotente repetida, a senha não é re-exibida.
/// </summary>
public sealed record SeedTenantCommandResult(
    Guid TenantId,
    Guid? AdminUserId,
    string? SenhaInicial,
    bool EhNovo);
