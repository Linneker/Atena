using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Services.V1.Fiscal.Services;

public interface INFeXmlBuilder
{
    /// <summary>
    /// Monta o XML simplificado da NF-e (layout 4.0 reduzido apenas com campos básicos).
    /// Em produção, substituir por implementação completa baseada em XSDs da SEFAZ ou biblioteca.
    /// </summary>
    string BuildEnvio(NFe nfe, IReadOnlyList<NFeItem> itens, ConfiguracaoFiscal config, string emitenteRazaoSocial);
}
