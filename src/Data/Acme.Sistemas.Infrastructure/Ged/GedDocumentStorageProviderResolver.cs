namespace Acme.Sistemas.Infrastructure.Ged;

public interface IGedDocumentStorageProviderResolver
{
    IGedStorageProvider Resolve(string providerName);
}

public sealed class GedDocumentStorageProviderResolver : IGedDocumentStorageProviderResolver
{
    private readonly IReadOnlyDictionary<string, IGedStorageProvider> _providers;

    public GedDocumentStorageProviderResolver(IEnumerable<IGedStorageProvider> providers)
    {
        _providers = providers.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }

    public IGedStorageProvider Resolve(string providerName)
    {
        if (!_providers.TryGetValue(providerName, out var provider))
            throw new InvalidOperationException($"Provedor de storage '{providerName}' não registrado.");
        return provider;
    }
}
