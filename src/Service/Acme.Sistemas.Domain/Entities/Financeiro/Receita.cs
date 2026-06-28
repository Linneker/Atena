using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class Receita : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public decimal Valor { get; set; }
    public bool ReceitaFixa { get; set; }
    public DateTime DataPrevistaRecebimento { get; set; }
    public Guid? CompetenciaId { get; set; }
    public Guid? CentroDeCustoId { get; set; }
    public Guid? ClienteId { get; set; }
    public Guid? OrigemVendaId { get; set; }
    /// <summary>Referência à receita template (ReceitaFixa=true) que originou esta instância via recorrência.</summary>
    public Guid? OrigemReceitaId { get; set; }

    public StatusPagamento StatusRecebimento { get; set; } = StatusPagamento.Pendente;
    public decimal? ValorRecebido { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }
    public string? ObservacaoRecebimento { get; set; }
}
