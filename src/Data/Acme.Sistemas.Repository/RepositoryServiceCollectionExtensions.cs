using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Repository;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddAcmeRepositories(this IServiceCollection services)
    {
        var assembly = typeof(RepositoryServiceCollectionExtensions).Assembly;

        var repoTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface
                        && t.Name.EndsWith("Repository", StringComparison.Ordinal));

        foreach (var implementation in repoTypes)
        {
            var iface = implementation.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementation.Name}");

            if (iface is not null)
            {
                services.AddScoped(iface, implementation);
            }
        }

        return services;
    }
}
