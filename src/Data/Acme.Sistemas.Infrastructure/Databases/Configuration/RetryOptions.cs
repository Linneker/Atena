namespace Acme.Sistemas.Infrastructure.Databases.Configuration;

public sealed class RetryOptions
{
    public const string SectionName = "Database:Retry";

    public int MaxAttempts { get; set; } = 3;
    public int BaseDelayMs { get; set; } = 200;
    public int MaxDelayMs { get; set; } = 5_000;
}
