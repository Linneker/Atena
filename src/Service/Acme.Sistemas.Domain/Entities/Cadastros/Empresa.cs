using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Cadastros;

public sealed class Empresa : BaseEntity
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public string? InscricaoEstadual { get; set; }
    public string? InscricaoMunicipal { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
    public Endereco Endereco { get; set; } = new();
}
