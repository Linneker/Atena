using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class Despesa : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public decimal Valor { get; set; }
    public bool DespesaFixa { get; set; }
    public DateTime DataVencimento { get; set; }
    public Guid? CompetenciaId { get; set; }
    public Guid? CentroDeCustoId { get; set; }
    public Guid? FornecedorId { get; set; }
    /// <summary>Referência à despesa template (DespesaFixa=true) que originou esta instância via recorrência.</summary>
    public Guid? OrigemDespesaId { get; set; }

    public StatusPagamento StatusPagamento { get; set; } = StatusPagamento.Pendente;
    public decimal? ValorPago { get; set; }
    public DateTime? DataPagamento { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }
    public string? ObservacaoPagamento { get; set; }

    public bool IsAtrasada =>
        StatusPagamento == StatusPagamento.Pendente && DataVencimento.Date < DateTime.UtcNow.Date;
}
