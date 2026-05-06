namespace Acme.Sistemas.Domain.Entities.Produtos;

public sealed class TipoValorProduto : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
}
