using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class ContaPagar : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public Guid? FornecedorId { get; set; }
    public Guid? DespesaId { get; set; }
    public Guid? PlanoDeContasId { get; set; }
    public decimal ValorOriginal { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public StatusConta Status { get; set; } = StatusConta.Pendente;
    public string? Observacao { get; set; }

    public decimal Saldo => ValorOriginal - ValorPago;

    public bool VencidaEm(DateTime data) =>
        Status != StatusConta.Pago && Status != StatusConta.Cancelado && DataVencimento.Date < data.Date;

    public bool VenceEmAteDias(int dias, DateTime referencia) =>
        Status == StatusConta.Pendente
        && DataVencimento.Date >= referencia.Date
        && DataVencimento.Date <= referencia.Date.AddDays(dias);
}
