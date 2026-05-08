using Acme.Sistemas.Domain.Interfaces.Messaging;
using Acme.Sistemas.Infrastructure.Messaging.Email;
using Acme.Sistemas.Infrastructure.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

public sealed class EmailDispatcherHostedService : BackgroundService
{
    private readonly IRabbitMqBus _bus;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailDispatcherHostedService> _logger;

    public EmailDispatcherHostedService(
        IRabbitMqBus bus,
        IServiceScopeFactory scopeFactory,
        ILogger<EmailDispatcherHostedService> logger)
    {
        _bus = bus;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var binding = new SubscribeBinding(
            Exchange: EmailQueueNames.Exchange,
            Queue: EmailQueueNames.Queue,
            RoutingKey: EmailQueueNames.RoutingKey,
            DeadLetterExchange: EmailQueueNames.DeadLetterExchange,
            DeadLetterQueue: EmailQueueNames.DeadLetterQueue,
            DeadLetterRoutingKey: EmailQueueNames.DeadLetterRoutingKey,
            MaxRetries: 5,
            PrefetchCount: 10);

        try
        {
            await _bus.SubscribeBoundAsync<EmailMessage>(
                binding,
                async (message, ct) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISmtpEmailSender>();
                    await sender.SendAsync(message, ct);
                },
                stoppingToken);

            _logger.LogInformation(
                "EmailDispatcherHostedService consumindo fila {Queue} (DLQ: {Dlq}).",
                binding.Queue, binding.DeadLetterQueue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao iniciar EmailDispatcherHostedService.");
        }

        await Task.Delay(Timeout.Infinite, stoppingToken).ContinueWith(_ => { });
    }
}
