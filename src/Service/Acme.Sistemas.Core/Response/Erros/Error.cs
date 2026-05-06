namespace Acme.Sistemas.Core.Response.Erros;

public sealed record Error(string Code, string Message)
{
    public static Error Validation(string message) => new("VALIDATION", message);
    public static Error NotFound(string message) => new("NOT_FOUND", message);
    public static Error Unauthorized(string message) => new("UNAUTHORIZED", message);
    public static Error Forbidden(string message) => new("FORBIDDEN", message);
    public static Error Conflict(string message) => new("CONFLICT", message);
    public static Error Internal(string message) => new("INTERNAL", message);
}
