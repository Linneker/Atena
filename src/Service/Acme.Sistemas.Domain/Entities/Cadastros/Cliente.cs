using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Cadastros;

public enum TipoPessoa
{
    Fisica = 1,
    Juridica = 2
}

public sealed class Cliente : BaseEntity
{
    public TipoPessoa Tipo { get; set; } = TipoPessoa.Juridica;
    public string Nome { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string? InscricaoEstadual { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
    public bool Inadimplente { get; set; }
    public bool BloqueadoVendas { get; set; }
    public Endereco Endereco { get; set; } = new();
}
