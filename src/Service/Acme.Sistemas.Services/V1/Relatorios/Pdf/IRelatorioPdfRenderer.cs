using Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

namespace Acme.Sistemas.Services.V1.Relatorios.Pdf;

public sealed record TenantBranding(string RazaoSocial, string? LogoUrl, string? CorPrimariaHex);

public interface IRelatorioPdfRenderer
{
    byte[] RenderDRE(DREResult dre, TenantBranding branding);
    byte[] RenderBalanco(BalancoResult balanco, TenantBranding branding);
}
