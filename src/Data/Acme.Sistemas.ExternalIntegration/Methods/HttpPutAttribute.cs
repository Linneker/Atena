namespace Acme.Sistemas.ExternalIntegration.Methods;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class HttpPutAttribute : Attribute
{
    public string Path { get; }
    public HttpPutAttribute(string path) { Path = path; }
}
