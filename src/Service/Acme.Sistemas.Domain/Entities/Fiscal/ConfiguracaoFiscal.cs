namespace Acme.Sistemas.Domain.Entities.Fiscal;

public enum AmbienteFiscal
{
    Homologacao = 2,
    Producao = 1
}

public enum ModoTransmissao
{
    Normal = 1,
    ContingenciaSvrs = 2
}

public sealed class ConfiguracaoFiscal : BaseEntity
{
    public AmbienteFiscal Ambiente { get; set; } = AmbienteFiscal.Homologacao;
    public ModoTransmissao Modo { get; set; } = ModoTransmissao.Normal;
    public string Uf { get; set; } = "SP";
    public string CnpjEmitente { get; set; } = string.Empty;
    public string? RazaoSocialEmitente { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int SerieNFe { get; set; } = 1;
    public int ProximoNumero { get; set; } = 1;
    public byte[]? CertificadoPfxCriptografado { get; set; }
    public string? CertificadoNonceBase64 { get; set; }
    public string? CertificadoSubject { get; set; }
    public DateTime? CertificadoValidoAte { get; set; }
    public string? CertificadoSenhaCriptografada { get; set; }
    public string? CertificadoSenhaNonceBase64 { get; set; }
}
