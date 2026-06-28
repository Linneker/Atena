namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class Cargo : BaseEntity
{
    public string? Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? CodigoCbo { get; set; }
    public decimal? SalarioBaseSugerido { get; set; }
    public bool Ativo { get; set; } = true;
}
