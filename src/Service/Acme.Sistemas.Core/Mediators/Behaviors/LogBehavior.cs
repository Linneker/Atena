using System.Diagnostics;
using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Core.Mediators.Handler;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Core.Mediators.Behaviors;

public sealed class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LogBehavior<TRequest, TResponse>> _logger;

    public LogBehavior(ILogger<LogBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var nome = typeof(TRequest).Name;
        var scope = LogEnrichmentHelper.Build();

        using (_logger.BeginScope(scope))
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Mediator >> {Request} iniciado", nome);
            try
            {
                var response = await next();
                stopwatch.Stop();
                _logger.LogInformation("Mediator << {Request} concluído em {DuracaoMs}ms",
                    nome, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Mediator !! {Request} falhou em {DuracaoMs}ms: {Mensagem}",
                    nome, stopwatch.ElapsedMilliseconds, ex.Message);
                throw;
            }
        }
    }
}
