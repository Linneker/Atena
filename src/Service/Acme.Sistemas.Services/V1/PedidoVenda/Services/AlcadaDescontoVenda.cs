namespace Acme.Sistemas.Services.V1.PedidoVenda.Services;

/// <summary>
/// Alçada de desconto em vendas: faixas de % com permissão crescente.
/// </summary>
public static class AlcadaDescontoVenda
{
    public const string PermissaoAte5 = "vendas:desconto:ate-5";
    public const string PermissaoAte15 = "vendas:desconto:ate-15";
    public const string PermissaoAcima15 = "vendas:desconto:acima-15";

    public static string PermissaoNecessaria(decimal percentual) => percentual switch
    {
        <= 5m => PermissaoAte5,
        <= 15m => PermissaoAte15,
        _ => PermissaoAcima15
    };

    public static bool TemAlcada(IReadOnlySet<string> permissoes, decimal percentual)
    {
        var necessaria = PermissaoNecessaria(percentual);
        if (permissoes.Contains(necessaria)) return true;
        if (necessaria == PermissaoAte5 && (permissoes.Contains(PermissaoAte15) || permissoes.Contains(PermissaoAcima15))) return true;
        if (necessaria == PermissaoAte15 && permissoes.Contains(PermissaoAcima15)) return true;
        return false;
    }
}
