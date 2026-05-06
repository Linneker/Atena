using System.Reflection;
using Acme.Sistemas.ExternalIntegration.Helper;

namespace Acme.Sistemas.ExternalIntegration.Proxys;

public interface IHttpClientProxyFactory
{
    TInterface Create<TInterface>(HttpClient httpClient) where TInterface : class, IExternalApiClient;
}

public sealed class HttpClientProxyFactory : IHttpClientProxyFactory
{
    public TInterface Create<TInterface>(HttpClient httpClient) where TInterface : class, IExternalApiClient
    {
        var proxy = DispatchProxy.Create<TInterface, HttpClientProxy<TInterface>>();
        ((HttpClientProxy<TInterface>)(object)proxy).Configure(httpClient);
        return proxy;
    }
}
