namespace Acme.Sistemas.Domain.Entities.Rh;

/// <summary>
/// Valor estruturado serializado em <c>funcionarios.conta_bancaria_json</c>.
/// Não é entidade persistente — só representação para serialização.
/// </summary>
public sealed class ContaBancariaFuncionario
{
    public string? CodigoBanco { get; set; }
    public string? NomeBanco { get; set; }
    public string? Agencia { get; set; }
    public string? AgenciaDigito { get; set; }
    public string? Conta { get; set; }
    public string? ContaDigito { get; set; }
    public string? TipoConta { get; set; }
    public string? ChavePix { get; set; }
}
