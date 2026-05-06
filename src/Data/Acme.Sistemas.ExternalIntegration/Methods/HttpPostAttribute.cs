namespace Acme.Sistemas.ExternalIntegration.Methods;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class HttpPostAttribute : Attribute
{
    public string Path { get; }
    public HttpPostAttribute(string path) { Path = path; }
}
