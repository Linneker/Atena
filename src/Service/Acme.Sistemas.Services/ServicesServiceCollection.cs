using Acme.Sistemas.Core;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz;
using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;
using Acme.Sistemas.Services.V1.Estoque.Services;
using Acme.Sistemas.Services.V1.Fiscal.Services;
using Acme.Sistemas.Services.V1.Rh.Mobile.Push;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
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

        // RH-Ponto W2: gerador de PDF do espelho mensal (QuestPDF)
        services.AddSingleton<IGeradorEspelhoPdf, GeradorEspelhoPdfQuestPdf>();

        // RH-Mobile W3: push notification (stub MVP; FCM/APNs reais em PR dedicada)
        services.AddSingleton<INotificacaoPushService, StubNotificacaoPushService>();

        // RH ponto-oficial-671 (W4): geradores de texto + PDF do comprovante de marcação
        services.AddSingleton<Acme.Sistemas.Domain.Interfaces.Rh.IGeradorComprovantePontoTexto,
            V1.Rh.Oficial671.Servicos.GeradorComprovantePontoTexto>();
        services.AddSingleton<Acme.Sistemas.Domain.Interfaces.Rh.IGeradorComprovantePontoPdf,
            V1.Rh.Oficial671.Servicos.GeradorComprovantePontoPdf>();
        services.AddScoped<V1.Rh.Oficial671.Servicos.IEmitirComprovante671,
            V1.Rh.Oficial671.Servicos.EmitirComprovante671>();
        services.AddSingleton<V1.Rh.Oficial671.Afd.LayoutAfd003Writer>();
        services.AddSingleton<V1.Rh.Oficial671.Aej.GeradorAejV1>();
        services.AddSingleton<V1.Rh.Oficial671.Aej.AssinadorAej>();

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
