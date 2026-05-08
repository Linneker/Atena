using System.Globalization;
using System.Text;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Fiscal;

namespace Acme.Sistemas.Services.V1.Fiscal.Services;

/// <summary>
/// Builder simplificado de XML NF-e — gera estrutura mínima para fluxo de teste.
/// Não substitui implementação real para SEFAZ.
/// </summary>
public sealed class NFeXmlBuilder : INFeXmlBuilder
{
    public string BuildEnvio(NFe nfe, IReadOnlyList<NFeItem> itens, ConfiguracaoFiscal config, string emitenteRazaoSocial)
    {
        var inv = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.Append(@"<NFe xmlns=""http://www.portalfiscal.inf.br/nfe"">");
        sb.Append($@"<infNFe Id=""NFe{nfe.ChaveAcesso}"" versao=""4.00"">");

        sb.Append("<ide>");
        sb.Append($"<cUF>{NFeChaveAcessoBuilder.CodigoUf(config.Uf):D2}</cUF>");
        sb.Append($"<natOp>VENDA</natOp>");
        sb.Append($"<mod>55</mod><serie>{nfe.Serie}</serie><nNF>{nfe.Numero}</nNF>");
        sb.Append($"<dhEmi>{nfe.DataEmissao:yyyy-MM-ddTHH:mm:sszzz}</dhEmi>");
        sb.Append($"<tpNF>1</tpNF><idDest>1</idDest><cMunFG>3550308</cMunFG>");
        sb.Append($"<tpImp>1</tpImp><tpEmis>{(int)nfe.Modo}</tpEmis><tpAmb>{(int)nfe.Ambiente}</tpAmb>");
        sb.Append($"<finNFe>1</finNFe><indFinal>1</indFinal><indPres>1</indPres><procEmi>0</procEmi>");
        sb.Append("</ide>");

        sb.Append("<emit>");
        sb.Append($"<CNPJ>{config.CnpjEmitente}</CNPJ>");
        sb.Append($"<xNome>{System.Security.SecurityElement.Escape(emitenteRazaoSocial)}</xNome>");
        sb.Append($"<IE>{config.InscricaoEstadual ?? "ISENTO"}</IE>");
        sb.Append("</emit>");

        sb.Append("<dest>");
        sb.Append($"<CPF>00000000000</CPF>");
        sb.Append($"<xNome>Cliente {nfe.ClienteId.ToString()[..8]}</xNome>");
        sb.Append("</dest>");

        var nItem = 1;
        foreach (var item in itens)
        {
            sb.Append($@"<det nItem=""{nItem++}"">");
            sb.Append("<prod>");
            sb.Append($"<cProd>{item.ProdutoId.ToString()[..8]}</cProd>");
            sb.Append($"<xProd>{System.Security.SecurityElement.Escape(item.Descricao)}</xProd>");
            sb.Append($"<NCM>{item.Ncm ?? "00000000"}</NCM>");
            sb.Append($"<CFOP>{item.Cfop ?? "5102"}</CFOP>");
            sb.Append($"<uCom>UN</uCom>");
            sb.Append($"<qCom>{item.Quantidade.ToString("0.0000", inv)}</qCom>");
            sb.Append($"<vUnCom>{item.PrecoUnitario.ToString("0.0000", inv)}</vUnCom>");
            sb.Append($"<vProd>{item.Total.ToString("0.00", inv)}</vProd>");
            sb.Append("</prod>");
            sb.Append("<imposto><ICMS><ICMS00><orig>0</orig><CST>00</CST><modBC>0</modBC><vBC>0.00</vBC><pICMS>0.00</pICMS><vICMS>0.00</vICMS></ICMS00></ICMS></imposto>");
            sb.Append("</det>");
        }

        sb.Append("<total><ICMSTot>");
        sb.Append($"<vBC>0.00</vBC><vICMS>0.00</vICMS><vProd>{nfe.ValorTotal.ToString("0.00", inv)}</vProd>");
        sb.Append($"<vNF>{nfe.ValorTotal.ToString("0.00", inv)}</vNF>");
        sb.Append("</ICMSTot></total>");

        sb.Append("<transp><modFrete>9</modFrete></transp>");

        sb.Append("</infNFe>");
        sb.Append("</NFe>");
        return sb.ToString();
    }
}
