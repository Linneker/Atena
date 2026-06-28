using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class Jornada : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public TipoJornada Tipo { get; set; }
    public decimal CargaSemanalHoras { get; set; }
    public decimal? CargaDiariaHoras { get; set; }
    public string JanelasJson { get; set; } = "[]";
    public bool PermiteMarcarIntervalo { get; set; } = true;
    public int ToleranciaMinutos { get; set; } = 10;
    public bool Ativo { get; set; } = true;
}
