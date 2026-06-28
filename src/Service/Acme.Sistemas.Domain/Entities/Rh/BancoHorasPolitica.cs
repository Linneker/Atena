namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class BancoHorasPolitica : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public DateOnly VigenciaInicio { get; set; }
    public DateOnly? VigenciaFim { get; set; }
    public decimal LimiteHorasAcumular { get; set; } = 40;
    public int PrazoCompensacaoDias { get; set; } = 180;
    public bool PermitePagarExcedente { get; set; } = true;
    public decimal FatorPagamento { get; set; } = 1.00m;
    public bool Ativo { get; set; } = true;
}
