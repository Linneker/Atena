using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;
using Acme.Sistemas.ExternalIntegration.Proxys;
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

        return services;
    }
}
