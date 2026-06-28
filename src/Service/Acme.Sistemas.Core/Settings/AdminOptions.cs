namespace Acme.Sistemas.Core.Settings;

/// <summary>
/// Configurações dos endpoints administrativos (<c>/api/v1/admin/*</c>).
/// </summary>
public sealed class AdminOptions
{
    public const string SectionName = "Admin";

    /// <summary>
    /// Lista de CIDRs autorizados a chamar rotas admin. Vazio = sem restrição de IP
    /// (loopback é sempre permitido). Em produção, recomenda-se redes privadas.
    /// </summary>
    public string[] AllowedIps { get; set; } = Array.Empty<string>();
}
