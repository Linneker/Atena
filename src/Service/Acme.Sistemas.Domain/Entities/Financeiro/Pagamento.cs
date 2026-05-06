using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class Pagamento : BaseEntity
{
    public Guid? DespesaId { get; set; }
    public Guid? DividaId { get; set; }
    public Guid? ContaPagarId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public string? Observacao { get; set; }
}
