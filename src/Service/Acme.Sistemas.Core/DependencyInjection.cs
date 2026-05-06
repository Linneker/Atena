using System.Reflection;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Mediators.Notification;
using Acme.Sistemas.Core.Security;
using Microsoft.Extensions.DependencyInjection;

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

        foreach (var assembly in assembliesToScan)
        {
            RegisterHandlers(services, assembly, typeof(IRequestHandler<,>));
            RegisterHandlers(services, assembly, typeof(INotificationHandler<>));
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

    private static void RegisterPipelineBehaviors(IServiceCollection services, Assembly assembly)
    {
        var behaviorOpen = typeof(IPipelineBehavior<,>);
        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var ifaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == behaviorOpen);

            foreach (var iface in ifaces)
            {
                services.AddScoped(iface, type);
            }
        }
    }
}
