using Acme.Sistemas.Core;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;
using Acme.Sistemas.Services.V1.Estoque.Services;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Services;

public static class ServicesServiceCollection
{
    public static IServiceCollection AddAcmeServices(this IServiceCollection services)
    {
        var assembly = typeof(ServicesServiceCollection).Assembly;

        // AddAcmeMediator descobre e registra:
        //   - IRequestHandler<,> e INotificationHandler<>
        //   - 4 behaviors transversais (Validation → CacheLookup → Audit → Log) closed por Command/Query
        //   - Behaviors específicos (não-transversais) implementando IPipelineBehavior<,>
        services.AddAcmeMediator(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<ConciliacaoMatcher>();
        services.AddScoped<FifoCustoCalculator>();

        // Fiscal NF-e — implementações que vivem em Services
        services.AddSingleton<INFeXmlBuilder, NFeXmlBuilder>();
        services.AddSingleton<INFeXmlSigner, StubNFeXmlSigner>();
        services.AddSingleton<INFeSefazClient, StubNFeSefazClient>();

        return services;
    }
}
