using System.Collections.Concurrent;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Mediators.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Core.Mediators;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _provider;
    private static readonly ConcurrentDictionary<Type, Type> HandlerCache = new();
    private static readonly ConcurrentDictionary<Type, Type> NotificationHandlerCache = new();

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var handlerType = HandlerCache.GetOrAdd(
            requestType,
            t => typeof(IRequestHandler<,>).MakeGenericType(t, typeof(TResponse)));

        var handler = _provider.GetRequiredService(handlerType);

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = ((IEnumerable<object>)_provider.GetServices(behaviorType)).Reverse().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
        {
            var method = handlerType.GetMethod("Handle")!;
            return (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;
        };

        foreach (var behavior in behaviors)
        {
            var current = handlerDelegate;
            var behaviorMethod = behaviorType.GetMethod("Handle")!;
            handlerDelegate = () => (Task<TResponse>)behaviorMethod.Invoke(
                behavior,
                new object[] { request, current, cancellationToken })!;
        }

        return await handlerDelegate();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlerType = NotificationHandlerCache.GetOrAdd(
            typeof(TNotification),
            t => typeof(INotificationHandler<>).MakeGenericType(t));

        var handlers = _provider.GetServices(handlerType);
        var tasks = handlers
            .Where(h => h is not null)
            .Select(h =>
            {
                var method = handlerType.GetMethod("Handle")!;
                return (Task)method.Invoke(h, new object[] { notification, cancellationToken })!;
            });

        await Task.WhenAll(tasks);
    }
}
