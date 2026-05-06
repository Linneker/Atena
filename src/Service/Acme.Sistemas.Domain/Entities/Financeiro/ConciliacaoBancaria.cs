namespace Acme.Sistemas.Domain.Entities.Financeiro;

public enum StatusConciliacao
{
    Importado = 0,
    EmRevisao = 1,
    Concluido = 2
}

public sealed class ConciliacaoBancaria : BaseEntity
{
    public string Banco { get; set; } = string.Empty;
    public string? Agencia { get; set; }
    public string? Conta { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFim { get; set; }
    public string FormatoArquivo { get; set; } = "CSV";
    public StatusConciliacao Status { get; set; } = StatusConciliacao.Importado;
    public int TotalLancamentos { get; set; }
    public int TotalConciliados { get; set; }
}

public enum TipoMovimentoExtrato
{
    Credito = 0,
    Debito = 1
}

public enum StatusItemExtrato
{
    NaoConciliado = 0,
    ConciliadoAutomaticamente = 1,
    ConciliadoManualmente = 2,
    Ignorado = 3
}

public sealed class ItemExtrato : BaseEntity
{
    public Guid ConciliacaoId { get; set; }
    public DateTime DataMovimento { get; set; }
    public decimal Valor { get; set; }
    public TipoMovimentoExtrato Tipo { get; set; }
    public string? Descricao { get; set; }
    public string? DocumentoBancario { get; set; }
    public StatusItemExtrato Status { get; set; } = StatusItemExtrato.NaoConciliado;
    public Guid? ContaPagarId { get; set; }
    public Guid? ContaReceberId { get; set; }
}
