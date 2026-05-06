namespace Acme.Sistemas.ExternalIntegration.Methods;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class HeaderAttribute : Attribute
{
    public string Name { get; }
    public string Value { get; }

    public HeaderAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
