using Acme.Sistemas.Core;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Services;

public static class ServicesServiceCollection
{
    public static IServiceCollection AddAcmeServices(this IServiceCollection services)
    {
        var assembly = typeof(ServicesServiceCollection).Assembly;

        services.AddAcmeMediator(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
