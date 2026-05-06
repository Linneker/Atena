using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class Divida : BaseEntity
{
    public string Credor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal ValorOriginal { get; set; }
    public decimal ValorPago { get; set; }
    public decimal? TaxaJurosMensal { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int NumeroParcelas { get; set; }
    public StatusConta Status { get; set; } = StatusConta.Pendente;

    public decimal Saldo => ValorOriginal - ValorPago;
}
