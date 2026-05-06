namespace Acme.Sistemas.ExternalIntegration.Methods;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class HttpGetAttribute : Attribute
{
    public string Path { get; }
    public HttpGetAttribute(string path) { Path = path; }
}
