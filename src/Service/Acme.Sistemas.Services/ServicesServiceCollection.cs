using Acme.Sistemas.Core;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.Behaviors;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;
using Acme.Sistemas.Services.V1.Estoque.Services;
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
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));

        services.AddScoped<ConciliacaoMatcher>();
        services.AddScoped<FifoCustoCalculator>();

        return services;
    }
}
