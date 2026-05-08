using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

public interface INFeXmlBuilder
{
    string BuildEnvio(NFe nfe, IReadOnlyList<NFeItem> itens, ConfiguracaoFiscal config, string emitenteRazaoSocial);
}
