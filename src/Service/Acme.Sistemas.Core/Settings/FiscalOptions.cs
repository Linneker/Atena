namespace Acme.Sistemas.Core.Settings;

public sealed class FiscalOptions
{
    public const string SectionName = "Fiscal";

    /// <summary>Chave mestra (base64 ou texto) usada para derivar chaves AES-GCM por tenant.</summary>
    public string MasterEncryptionKey { get; set; } = "DEV-ONLY-CHANGE-IN-PROD-AT-LEAST-32-CHARS!";

    /// <summary>Limite de NF-e autorizadas por mês — 0 = sem limite. Sobrescrito por tenant via TenantLimites.MaxNFeMes.</summary>
    public int LimitePadraoNFePorMes { get; set; } = 0;

    /// <summary>Provider de storage para XMLs autorizados. Valores aceitos: "Local", "AwsS3".</summary>
    public string XmlStorageProvider { get; set; } = "Local";

    /// <summary>Bucket AWS S3 para armazenar XMLs (obrigatório quando XmlStorageProvider = "AwsS3").</summary>
    public string? AwsS3BucketXmls { get; set; }

    /// <summary>Dias de antecedência para emitir alerta de certificado a vencer.</summary>
    public int DiasAlertaCertificadoVencer { get; set; } = 30;

    /// <summary>Periodicidade da varredura de certificados a vencer (em horas).</summary>
    public int IntervaloVarreduraCertificadosHoras { get; set; } = 24;
}
