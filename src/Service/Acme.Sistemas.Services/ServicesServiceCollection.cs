using Acme.Sistemas.Core;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;
using Acme.Sistemas.Services.V1.Estoque.Services;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Services;

public static class ServicesServiceCollection
{
    public static IServiceCollection AddAcmeServices(this IServiceCollection services, IConfiguration? configuration = null)
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

        // Cliente SEFAZ: default = Real (Fase 6 do nfe-cliente-sefaz-proprio).
        // Stub fica disponível como fallback emergencial em dev via flag Fiscal:UseStub=true.
        var useStub = configuration?.GetValue<bool>("Fiscal:UseStub") ?? false;
        if (useStub)
        {
            services.AddSingleton<INFeSefazClient, StubNFeSefazClient>();
        }
        else
        {
            services.AddScoped<INFeSefazClient>(sp => sp.GetRequiredService<RealNFeSefazClient>());
        }

        return services;
    }
}
