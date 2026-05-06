using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class PlanoDeContas : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public TipoConta Tipo { get; set; }
    public Guid? PaiId { get; set; }
    public int Nivel { get; set; }
    public bool Aceita_Lancamento { get; set; } = true;
    public bool Ativo { get; set; } = true;
}
