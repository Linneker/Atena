using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Rh;
using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;
using Acme.Sistemas.ExternalIntegration.Rh.Oficial671;
using Acme.Sistemas.ExternalIntegration.Proxys;
using Acme.Sistemas.ExternalIntegration.Sefaz;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.ExternalIntegration.Sefaz.Contingencia;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.ExternalIntegration;

public static class ExternalIntegrationDI
{
    public static IServiceCollection AddAcmeExternalIntegration(this IServiceCollection services)
    {
        services.AddSingleton<IHttpClientProxyFactory, HttpClientProxyFactory>();

        services.AddHttpClient("ViaCep", c => c.BaseAddress = new Uri("https://viacep.com.br/"));
        services.AddScoped<IViaCepExternalClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var proxyFactory = sp.GetRequiredService<IHttpClientProxyFactory>();
            return proxyFactory.Create<IViaCepExternalClient>(factory.CreateClient("ViaCep"));
        });

        // SEFAZ NF-e — blocos das Fases 1-5 do nfe-cliente-sefaz-proprio
        services.AddSingleton<SefazUrlCatalog>();
        services.AddSingleton<XsdValidator>();
        services.AddSingleton<XmlSignerC14N>();
        services.AddSingleton<ContingenciaPolicy>();
        services.AddSingleton<ICertificadoLoader>(_ => new A1CertificadoLoader(validarCadeiaIcpBrasil: true));
        services.AddScoped<CertificadoTenantResolver>();
        services.AddScoped<SefazSoapClient>();
        services.AddScoped<NFeAutorizacaoService>();
        services.AddScoped<NFeRetAutorizacaoService>();
        services.AddScoped<NFeConsultaProtocoloService>();
        services.AddScoped<NFeStatusServicoService>();
        services.AddScoped<NFeRecepcaoEventoService>();
        services.AddScoped<NFeInutilizacaoService>();
        services.AddScoped<RealNFeSefazClient>();

        // RH ponto-oficial-671: assinador ICP-Brasil do comprovante de marcação
        services.AddSingleton<IAssinadorComprovante671, AssinadorComprovante671>();

        return services;
    }
}
