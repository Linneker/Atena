namespace Acme.Sistemas.Domain.Entities.Vendas;

public enum StatusOrcamento
{
    Rascunho = 0,
    Enviado = 1,
    Aprovado = 2,
    Rejeitado = 3,
    Expirado = 4,
    ConvertidoEmPedido = 5
}

public sealed class Orcamento : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public Guid? VendedorId { get; set; }
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
    public DateTime DataValidade { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal? DescontoPercentual { get; set; }
    public StatusOrcamento Status { get; set; } = StatusOrcamento.Rascunho;
    public string? Observacao { get; set; }
    public List<OrcamentoItem> Itens { get; set; } = new();
}

public sealed class OrcamentoItem : BaseEntity
{
    public Guid OrcamentoId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
}
