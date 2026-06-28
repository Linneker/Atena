namespace Acme.Sistemas.Domain.Entities.Rh;

/// <summary>
/// Feriado nacional/estadual/municipal. Quando <c>TenantId</c> é null, é feriado nacional
/// e vale para todos os tenants; quando preenchido, é feriado próprio do tenant.
/// </summary>
public sealed class Feriado : BaseEntity
{
    public DateOnly Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Tipo { get; set; } = "Nacional";
    public string? Uf { get; set; }
    public string? MunicipioIbge { get; set; }
    public bool Ativo { get; set; } = true;
}
