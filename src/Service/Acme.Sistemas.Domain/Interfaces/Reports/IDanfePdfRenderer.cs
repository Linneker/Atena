using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Reports;

public sealed record DanfeData(
    NFe NFe,
    IReadOnlyList<NFeItem> Itens,
    string EmitenteRazaoSocial,
    string EmitenteCnpj,
    string ClienteNome);

public interface IDanfePdfRenderer
{
    byte[] Render(DanfeData data, TenantBranding branding);
}
