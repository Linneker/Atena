namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class CentroDeCusto : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public Guid? ResponsavelId { get; set; }
    public bool Ativo { get; set; } = true;
}
