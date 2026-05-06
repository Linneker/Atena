namespace Acme.Sistemas.Domain.Entities.Permissions;

public sealed class Permission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Recurso { get; set; } = string.Empty;
    public string Acao { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
