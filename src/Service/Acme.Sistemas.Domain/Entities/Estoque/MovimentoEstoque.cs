namespace Acme.Sistemas.Domain.Entities.Estoque;

public enum TipoMovimentoEstoque
{
    Entrada = 1,
    Saida = 2,
    AjusteEntrada = 3,
    AjusteSaida = 4,
    Reserva = 5,
    LiberacaoReserva = 6
}

public enum OrigemMovimento
{
    Manual = 0,
    Compra = 1,
    Venda = 2,
    Devolucao = 3,
    Inventario = 4,
    TransferenciaEntreEstoques = 5
}

public sealed class EntradaProdutoEstoque : BaseEntity
{
    public Guid EstoqueId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal QuantidadeRestante { get; set; }
    public decimal? CustoUnitario { get; set; }
    public OrigemMovimento Origem { get; set; } = OrigemMovimento.Manual;
    public string? Motivo { get; set; }
    public Guid? FornecedorId { get; set; }
    public string? DocumentoReferencia { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
}

public sealed class SaidaProdutoEstoque : BaseEntity
{
    public Guid EstoqueId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal? CustoUnitario { get; set; }
    public decimal? CmvUnitario { get; set; }
    public OrigemMovimento Origem { get; set; } = OrigemMovimento.Manual;
    public string? Motivo { get; set; }
    public Guid? ClienteId { get; set; }
    public string? DocumentoReferencia { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
}
