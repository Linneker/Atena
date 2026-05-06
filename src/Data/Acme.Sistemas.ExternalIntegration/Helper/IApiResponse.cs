namespace Acme.Sistemas.ExternalIntegration.Helper;

public interface IApiResponse
{
    int StatusCode { get; }
    bool IsSuccess { get; }
    string? ErrorMessage { get; }
}

public interface IApiResponse<T> : IApiResponse
{
    T? Content { get; }
}
