namespace Acme.Sistemas.Domain.Entities.Fiscal;

public enum StatusNFe
{
    Rascunho = 0,
    AguardandoTransmissao = 1,
    Transmitindo = 2,
    Autorizada = 3,
    Rejeitada = 4,
    Cancelada = 5,
    Denegada = 6,
    EmContingencia = 7
}

public sealed class NFe : BaseEntity
{
    public int Numero { get; set; }
    public int Serie { get; set; }
    public string? ChaveAcesso { get; set; }
    public Guid? FaturamentoId { get; set; }
    public Guid ClienteId { get; set; }
    public AmbienteFiscal Ambiente { get; set; }
    public ModoTransmissao Modo { get; set; } = ModoTransmissao.Normal;
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAutorizacao { get; set; }
    public StatusNFe Status { get; set; } = StatusNFe.Rascunho;
    public string? ProtocoloAutorizacao { get; set; }
    public string? CodigoStatusSefaz { get; set; }
    public string? MotivoSefaz { get; set; }
    public decimal ValorTotal { get; set; }
    public string? XmlAutorizadoUrl { get; set; }
    public string? XmlEnviadoHash { get; set; }
    public List<NFeItem> Itens { get; set; } = new();
}

public sealed class NFeItem : BaseEntity
{
    public Guid NFeId { get; set; }
    public int NumeroItem { get; set; }
    public Guid ProdutoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
    public string? Cfop { get; set; }
    public string? Ncm { get; set; }
}

public enum TipoEventoNFe
{
    Cancelamento = 110111,
    CartaCorrecao = 110110
}

public sealed class NFeEvento : BaseEntity
{
    public Guid NFeId { get; set; }
    public TipoEventoNFe Tipo { get; set; }
    public int Sequencia { get; set; } = 1;
    public DateTime DataEvento { get; set; } = DateTime.UtcNow;
    public string? Descricao { get; set; }
    public string? ProtocoloAutorizacao { get; set; }
    public string? CodigoStatusSefaz { get; set; }
    public string? MotivoSefaz { get; set; }
    public string? XmlEventoUrl { get; set; }
}
