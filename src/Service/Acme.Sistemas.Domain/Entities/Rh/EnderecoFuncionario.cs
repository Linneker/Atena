namespace Acme.Sistemas.Domain.Entities.Rh;

/// <summary>
/// Valor estruturado serializado em <c>funcionarios.endereco_json</c>.
/// Não é entidade persistente — só representação para serialização.
/// </summary>
public sealed class EnderecoFuncionario
{
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Pais { get; set; } = "BR";
}
