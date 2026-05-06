namespace Acme.Sistemas.ExternalIntegration.Helper;

public sealed record ApiResponse<T>(
    int StatusCode,
    T? Content,
    string? ErrorMessage = null) : IApiResponse<T>
{
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
}

public sealed record ApiResponse(
    int StatusCode,
    string? ErrorMessage = null) : IApiResponse
{
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
}
