using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class ContaReceber : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public Guid? ClienteId { get; set; }
    public Guid? ReceitaId { get; set; }
    public Guid? PlanoDeContasId { get; set; }
    public decimal ValorOriginal { get; set; }
    public decimal ValorRecebido { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public StatusConta Status { get; set; } = StatusConta.Pendente;
    public string? ObservacaoRecebimento { get; set; }

    public decimal Saldo => ValorOriginal - ValorRecebido;

    public int DiasAtrasoEm(DateTime referencia) =>
        Status == StatusConta.Pago || Status == StatusConta.Cancelado
            ? 0
            : Math.Max(0, (referencia.Date - DataVencimento.Date).Days);
}
