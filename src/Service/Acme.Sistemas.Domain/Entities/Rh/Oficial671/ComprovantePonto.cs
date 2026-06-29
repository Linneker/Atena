namespace Acme.Sistemas.Domain.Entities.Rh.Oficial671;

/// <summary>
/// Comprovante de marcação Portaria 671/2021 anexo II.
/// Payload texto fixo + assinatura ICP-Brasil RSA-SHA-256 + hash SHA-256.
/// </summary>
public sealed class ComprovantePonto : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public Guid MarcacaoId { get; set; }
    public long Nsr { get; set; }
    public string PayloadTexto { get; set; } = string.Empty;
    public string AssinaturaBase64 { get; set; } = string.Empty;
    public string HashSha256 { get; set; } = string.Empty;
    public string? CertificadoThumbprint { get; set; }
    public DateTime EmitidoEm { get; set; } = DateTime.UtcNow;
}
