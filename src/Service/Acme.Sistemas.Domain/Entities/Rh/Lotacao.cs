namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class Lotacao : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public Guid? EmpresaId { get; set; }
    public string? Cnpj { get; set; }
    public string? EnderecoJson { get; set; }
    public bool Ativo { get; set; } = true;
}
