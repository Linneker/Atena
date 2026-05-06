namespace Acme.Sistemas.Domain.Entities.Estoque;

public sealed class Estoque : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public bool PermiteSaldoNegativo { get; set; }
    public bool Ativo { get; set; } = true;
}
