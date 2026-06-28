using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class BeneficioCatalogo : BaseEntity
{
    public string? Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public TipoBeneficio Tipo { get; set; }
    public decimal? DescontoFuncionarioPct { get; set; }
    public decimal? CustoEmpresaPadrao { get; set; }
    public string? NaturezaRubricaEsocial { get; set; }
    public bool Ativo { get; set; } = true;
}
