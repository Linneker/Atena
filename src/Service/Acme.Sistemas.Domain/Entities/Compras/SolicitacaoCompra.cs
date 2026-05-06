namespace Acme.Sistemas.Domain.Entities.Compras;

public enum StatusSolicitacaoCompra
{
    Rascunho = 0,
    AguardandoAprovacao = 1,
    Aprovada = 2,
    Rejeitada = 3,
    Cancelada = 4,
    ConvertidaEmPedido = 5
}

public sealed class SolicitacaoCompra : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public Guid? SolicitanteId { get; set; }
    public string? Justificativa { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    public StatusSolicitacaoCompra Status { get; set; } = StatusSolicitacaoCompra.Rascunho;
    public Guid? AprovadoPor { get; set; }
    public DateTime? AprovadoEm { get; set; }
    public string? MotivoRejeicao { get; set; }
    public List<SolicitacaoCompraItem> Itens { get; set; } = new();
}

public sealed class SolicitacaoCompraItem : BaseEntity
{
    public Guid SolicitacaoCompraId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal? PrecoEstimado { get; set; }
    public string? Observacao { get; set; }
}
