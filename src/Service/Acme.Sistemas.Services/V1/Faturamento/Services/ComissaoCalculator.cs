namespace Acme.Sistemas.Services.V1.Faturamento.Services;

/// <summary>
/// Cálculo de comissão simples — percentual fixo sobre valor faturado.
/// Em produção, faixas seriam configuráveis por tenant/vendedor/produto.
/// </summary>
public static class ComissaoCalculator
{
    public const decimal PercentualPadrao = 3m;

    public static decimal Calcular(decimal valorFaturado, decimal? percentualOverride = null)
    {
        var perc = percentualOverride ?? PercentualPadrao;
        return Math.Round(valorFaturado * perc / 100m, 2);
    }
}
