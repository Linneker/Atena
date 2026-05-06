using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Produtos;

public sealed class Produto : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? CodigoBarras { get; set; }
    public string UnidadeMedida { get; set; } = "UN";
    public Guid? TipoProdutoId { get; set; }
    public Guid? FornecedorId { get; set; }
    public decimal? CustoMedio { get; set; }
    public decimal? EstoqueMinimo { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
    public List<ValorProduto> Precos { get; set; } = new();
}

public sealed class ValorProduto : BaseEntity
{
    public Guid ProdutoId { get; set; }
    public Guid TipoValorProdutoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime VigenciaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? VigenciaFim { get; set; }
}
