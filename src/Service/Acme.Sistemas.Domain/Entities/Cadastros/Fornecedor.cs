using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Cadastros;

public sealed class Fornecedor : BaseEntity
{
    public TipoPessoa Tipo { get; set; } = TipoPessoa.Juridica;
    public string Nome { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string? InscricaoEstadual { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? CondicaoPagamentoPadrao { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
    public Endereco Endereco { get; set; } = new();
}
