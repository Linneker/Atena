using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Domain.Interfaces.Reports;

public sealed record TenantBranding(string RazaoSocial, string? LogoUrl, string? CorPrimariaHex);

public interface IRelatorioPdfRenderer
{
    byte[] RenderDRE(DREResult dre, TenantBranding branding);
    byte[] RenderBalanco(BalancoResult balanco, TenantBranding branding);
}
