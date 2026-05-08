using System.Reflection;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Behaviors;
using Acme.Sistemas.Core.Mediators.Cache;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Mediators.Notification;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Interfaces.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acme.Sistemas.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddAcmeSecurity(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        return services;
    }

    public static IServiceCollection AddAcmeMediator(this IServiceCollection services, params Assembly[] assembliesToScan)
    {
        services.AddScoped<IMediator, Mediator>();

        // Mock provisório de ICacheStore — usado em testes/cenários sem Infrastructure.
        // Em produção, Infrastructure registra CacheProviderRouter (Hybrid + Redis) que sobrescreve.
        services.TryAddSingleton<ICacheStore, InMemoryCacheStore>();

        foreach (var assembly in assembliesToScan)
        {
            RegisterHandlers(services, assembly, typeof(IRequestHandler<,>));
            RegisterHandlers(services, assembly, typeof(INotificationHandler<>));
            RegisterTransversalBehaviorsClosed(services, assembly);
            RegisterPipelineBehaviors(services, assembly);
        }

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly, Type openGenericInterface)
    {
        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var implementedInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterface);

            foreach (var iface in implementedInterfaces)
            {
                services.AddScoped(iface, type);
            }
        }
    }

    /// <summary>
    /// Registra os 4 behaviors transversais (Validation → CacheLookup → Audit → Log) como tipos
    /// fechados por Command/Query do assembly. A ordem de registro define a ordem de execução
    /// (primeiro registrado = mais externo = executa primeiro).
    /// </summary>
    private static void RegisterTransversalBehaviorsClosed(IServiceCollection services, Assembly assembly)
    {
        var requestInterface = typeof(IRequest<>);
        var pipelineInterface = typeof(IPipelineBehavior<,>);
        var transversais = new[]
        {
            typeof(ValidationBehavior<,>),
            typeof(CacheLookupBehavior<,>),
            typeof(AuditBehavior<,>),
            typeof(LogBehavior<,>),
        };

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition) continue;

            var iReq = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == requestInterface);
            if (iReq is null) continue;

            var tRequest = type;
            var tResponse = iReq.GetGenericArguments()[0];
            var closedPipeline = pipelineInterface.MakeGenericType(tRequest, tResponse);

            foreach (var transversal in transversais)
            {
                services.AddScoped(closedPipeline, transversal.MakeGenericType(tRequest, tResponse));
            }
        }
    }

    private static void RegisterPipelineBehaviors(IServiceCollection services, Assembly assembly)
    {
        var behaviorOpen = typeof(IPipelineBehavior<,>);
        var transversais = new HashSet<Type>
        {
            typeof(ValidationBehavior<,>),
            typeof(CacheLookupBehavior<,>),
            typeof(AuditBehavior<,>),
            typeof(LogBehavior<,>),
        };

        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            // Open generic behaviors (com type parameters) ficam por conta de quem
            // chama o AddAcmeMediator — eles devem ser registrados closed por Command/Query
            // (constraint `where TRequest : IRequest<TResponse>` impede open generic
            // registration no DI do .NET 10 em CallSiteFactory.Populate).
            if (type.IsGenericTypeDefinition) continue;

            // Behaviors transversais já são registrados em RegisterTransversalBehaviorsClosed.
            if (type.IsGenericType && transversais.Contains(type.GetGenericTypeDefinition())) continue;

            var ifaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == behaviorOpen);

            foreach (var iface in ifaces)
            {
                services.AddScoped(iface, type);
            }
        }
    }
}
