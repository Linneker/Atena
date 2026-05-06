using Acme.Sistemas.Core.Response.Erros;

namespace Acme.Sistemas.Core.Response;

public sealed record ResponseDefault<T>
{
    public int Status { get; init; }
    public string? Message { get; init; }
    public T? Content { get; init; }
    public IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();

    public bool IsSuccess => Status >= 200 && Status < 300;

    public static ResponseDefault<T> Ok(T content, string? message = null) => new()
    {
        Status = 200,
        Content = content,
        Message = message
    };

    public static ResponseDefault<T> Created(T content, string? message = null) => new()
    {
        Status = 201,
        Content = content,
        Message = message
    };

    public static ResponseDefault<T> BadRequest(params Error[] errors) => new()
    {
        Status = 400,
        Errors = errors,
        Message = errors.FirstOrDefault()?.Message
    };

    public static ResponseDefault<T> NotFound(string message) => new()
    {
        Status = 404,
        Message = message,
        Errors = new[] { Error.NotFound(message) }
    };

    public static ResponseDefault<T> Conflict(string message) => new()
    {
        Status = 409,
        Message = message,
        Errors = new[] { Error.Conflict(message) }
    };

    public static ResponseDefault<T> Internal(string message) => new()
    {
        Status = 500,
        Message = message,
        Errors = new[] { Error.Internal(message) }
    };
}

public sealed record ResponseDefault
{
    public int Status { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();

    public bool IsSuccess => Status >= 200 && Status < 300;

    public static ResponseDefault Ok(string? message = null) => new() { Status = 200, Message = message };
    public static ResponseDefault NoContent() => new() { Status = 204 };
    public static ResponseDefault BadRequest(params Error[] errors) => new()
    {
        Status = 400,
        Errors = errors,
        Message = errors.FirstOrDefault()?.Message
    };
}
