namespace Acme.Sistemas.Core.Helper;

public static class LogEnrichmentHelper
{
    public const string TenantId = "TenantId";
    public const string UserId = "UserId";
    public const string CorrelationId = "CorrelationId";
    public const string RequestPath = "RequestPath";

    public static IDictionary<string, object?> Build(
        Guid? tenantId = null,
        Guid? userId = null,
        string? correlationId = null,
        string? requestPath = null)
    {
        var dict = new Dictionary<string, object?>();
        if (tenantId is not null) dict[TenantId] = tenantId;
        if (userId is not null) dict[UserId] = userId;
        if (!string.IsNullOrEmpty(correlationId)) dict[CorrelationId] = correlationId;
        if (!string.IsNullOrEmpty(requestPath)) dict[RequestPath] = requestPath;
        return dict;
    }
}
