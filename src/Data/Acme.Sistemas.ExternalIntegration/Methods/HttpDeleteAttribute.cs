namespace Acme.Sistemas.ExternalIntegration.Methods;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class HttpDeleteAttribute : Attribute
{
    public string Path { get; }
    public HttpDeleteAttribute(string path) { Path = path; }
}
