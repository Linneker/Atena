namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Services;

/// <summary>
/// Alçada de aprovação por valor (faixas hardcoded; em produção viria de configuração por tenant).
/// </summary>
public static class AlcadaAprovacao
{
    public const string PermissaoAte10k = "compras:aprovar:ate-10k";
    public const string PermissaoAte50k = "compras:aprovar:ate-50k";
    public const string PermissaoAcima50k = "compras:aprovar:acima-50k";

    public static string PermissaoNecessaria(decimal valor) => valor switch
    {
        <= 10_000m => PermissaoAte10k,
        <= 50_000m => PermissaoAte50k,
        _ => PermissaoAcima50k
    };

    public static bool TemAlcada(IReadOnlySet<string> permissoes, decimal valor)
    {
        // Hierarquia: quem aprova faixa maior também aprova menores.
        var necessaria = PermissaoNecessaria(valor);
        if (permissoes.Contains(necessaria)) return true;
        if (necessaria == PermissaoAte10k && (permissoes.Contains(PermissaoAte50k) || permissoes.Contains(PermissaoAcima50k))) return true;
        if (necessaria == PermissaoAte50k && permissoes.Contains(PermissaoAcima50k)) return true;
        return false;
    }
}
