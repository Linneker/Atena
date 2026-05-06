namespace Acme.Sistemas.Domain.Entities.Estoque;

/// <summary>Saldo agregado de um produto em um estoque.</summary>
public sealed class EstoqueProduto : BaseEntity
{
    public Guid EstoqueId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal SaldoTotal { get; set; }
    public decimal SaldoReservado { get; set; }
    public decimal SaldoDisponivel => SaldoTotal - SaldoReservado;
}
