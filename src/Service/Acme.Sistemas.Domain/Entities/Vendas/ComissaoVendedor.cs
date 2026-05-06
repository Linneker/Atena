namespace Acme.Sistemas.Domain.Entities.Vendas;

public enum StatusComissao
{
    Pendente = 0,
    Paga = 1,
    Cancelada = 2
}

public sealed class ComissaoVendedor : BaseEntity
{
    public Guid VendedorId { get; set; }
    public Guid FaturamentoId { get; set; }
    public decimal BaseCalculoValor { get; set; }
    public decimal PercentualComissao { get; set; }
    public decimal ValorComissao { get; set; }
    public DateTime DataReferencia { get; set; } = DateTime.UtcNow;
    public StatusComissao Status { get; set; } = StatusComissao.Pendente;
    public DateTime? DataPagamento { get; set; }
}
