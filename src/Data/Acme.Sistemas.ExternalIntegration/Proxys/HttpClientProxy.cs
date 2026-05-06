using System.Reflection;
using System.Text;
using System.Text.Json;
using Acme.Sistemas.ExternalIntegration.Helper;
using Acme.Sistemas.ExternalIntegration.Methods;

namespace Acme.Sistemas.ExternalIntegration.Proxys;

public sealed class HttpClientProxy<TInterface> : DispatchProxy where TInterface : class, IExternalApiClient
{
    private HttpClient _httpClient = null!;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    internal void Configure(HttpClient httpClient) => _httpClient = httpClient;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null) throw new InvalidOperationException("Método inválido.");

        var (httpMethod, path) = ResolveRoute(targetMethod);
        var url = BuildUrl(path, targetMethod, args);
        var headers = ResolveHeaders(targetMethod);

        var task = SendAsync(targetMethod, httpMethod, url, headers, args);
        return task;
    }

    private async Task<object?> SendAsync(
        MethodInfo method,
        HttpMethod httpMethod,
        string url,
        IReadOnlyList<(string Name, string Value)> headers,
        object?[]? args)
    {
        using var request = new HttpRequestMessage(httpMethod, url);

        foreach (var (name, value) in headers)
        {
            request.Headers.TryAddWithoutValidation(name, value);
        }

        if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && args is { Length: > 0 })
        {
            var bodyParam = method.GetParameters().LastOrDefault();
            if (bodyParam is not null && args.Length > 0)
            {
                var json = JsonSerializer.Serialize(args[^1], JsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        var response = await _httpClient.SendAsync(request);
        var status = (int)response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();

        var returnType = method.ReturnType;
        if (!returnType.IsGenericType) return null;
        var inner = returnType.GetGenericArguments()[0];

        if (!inner.IsGenericType || inner.GetGenericTypeDefinition() != typeof(ApiResponse<>))
        {
            if (inner == typeof(ApiResponse))
            {
                return new ApiResponse(status, response.IsSuccessStatusCode ? null : responseBody);
            }
            throw new NotSupportedException($"Tipo de retorno {inner.Name} não suportado pelo HttpClientProxy.");
        }

        var contentType = inner.GetGenericArguments()[0];
        object? content = null;

        if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseBody))
        {
            content = JsonSerializer.Deserialize(responseBody, contentType, JsonOptions);
        }

        var apiResponseType = typeof(ApiResponse<>).MakeGenericType(contentType);
        var apiResponse = Activator.CreateInstance(apiResponseType, status, content,
            response.IsSuccessStatusCode ? null : responseBody);
        return apiResponse;
    }

    private static (HttpMethod Method, string Path) ResolveRoute(MethodInfo method)
    {
        var get = method.GetCustomAttribute<HttpGetAttribute>();
        if (get is not null) return (HttpMethod.Get, get.Path);

        var post = method.GetCustomAttribute<HttpPostAttribute>();
        if (post is not null) return (HttpMethod.Post, post.Path);

        var put = method.GetCustomAttribute<HttpPutAttribute>();
        if (put is not null) return (HttpMethod.Put, put.Path);

        var del = method.GetCustomAttribute<HttpDeleteAttribute>();
        if (del is not null) return (HttpMethod.Delete, del.Path);

        throw new InvalidOperationException($"Método {method.Name} sem atributo HTTP definido.");
    }

    private static string BuildUrl(string path, MethodInfo method, object?[]? args)
    {
        if (args is null || args.Length == 0) return path;
        var parameters = method.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            var token = "{" + parameters[i].Name + "}";
            if (path.Contains(token))
            {
                path = path.Replace(token, args[i]?.ToString() ?? string.Empty);
            }
        }
        return path;
    }

    private static IReadOnlyList<(string, string)> ResolveHeaders(MethodInfo method)
    {
        var headers = new List<(string, string)>();
        foreach (var attr in method.GetCustomAttributes<HeaderAttribute>())
        {
            headers.Add((attr.Name, attr.Value));
        }
        var declaring = method.DeclaringType;
        if (declaring is not null)
        {
            foreach (var attr in declaring.GetCustomAttributes<HeaderAttribute>())
            {
                headers.Add((attr.Name, attr.Value));
            }
        }
        return headers;
    }
}
