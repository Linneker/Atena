namespace Acme.Sistemas.Core.Mediators.Handler;

public interface IRequest<out TResponse>
{
}

public interface IRequest : IRequest<Unit>
{
}

public readonly record struct Unit
{
    public static readonly Unit Value = new();
}
