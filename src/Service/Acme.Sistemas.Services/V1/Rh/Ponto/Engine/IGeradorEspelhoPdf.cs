namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Renderiza um espelho mensal em PDF. Implementação real fica em
/// Acme.Sistemas.Infrastructure usando QuestPDF (Infrastructure já referencia o pacote).
/// </summary>
public interface IGeradorEspelhoPdf
{
    byte[] Gerar(GeradorEspelhoMensal.EspelhoMensal espelho, string tenantRazaoSocial, string? logoUrl = null);
}
