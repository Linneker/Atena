using Acme.Sistemas.Infrastructure.Databases.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Infrastructure.Databases.Configuration;

public sealed class RetryPolicy
{
    private readonly RetryOptions _options;

    public RetryPolicy(IOptions<RetryOptions> options)
    {
        _options = options.Value;
    }

    public async Task<T> ExecuteAsync<T>(
        Func<Task<T>> action,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var attempt = 0;
        while (true)
        {
            attempt++;
            try
            {
                return await action();
            }
            catch (Exception ex) when (TransientErrorDetector.IsTransient(ex) && attempt < _options.MaxAttempts)
            {
                var delay = Math.Min(_options.BaseDelayMs * (int)Math.Pow(2, attempt - 1), _options.MaxDelayMs);
                logger.LogWarning(ex, "Erro transiente no banco. Tentativa {Attempt}/{Max}. Aguardando {Delay}ms.",
                    attempt, _options.MaxAttempts, delay);
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
