namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class SmtpOptions
{
    public const string SectionName = "Smtp";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 587;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool UseStartTls { get; set; } = true;
    public string FromAddress { get; set; } = "no-reply@atena.local";
    public string FromDisplayName { get; set; } = "Atena ERP";
    public int TimeoutSeconds { get; set; } = 30;
}
