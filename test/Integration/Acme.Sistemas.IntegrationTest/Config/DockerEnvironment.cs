using Testcontainers.MySql;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Config;

public sealed class DockerEnvironment : IAsyncLifetime
{
    private MySqlContainer? _mysql;

    public bool IsAvailable { get; private set; }

    public string? UnavailableReason { get; private set; }

    public string MySqlConnectionString =>
        _mysql?.GetConnectionString()
            ?? throw new InvalidOperationException(
                $"Docker não disponível: {UnavailableReason}");

    public async Task InitializeAsync()
    {
        try
        {
            _mysql = new MySqlBuilder()
                .WithImage("mysql:8.0")
                .WithDatabase("atena_test")
                .WithUsername("root")
                .WithPassword("root")
                .Build();

            await _mysql.StartAsync();
            IsAvailable = true;
        }
        catch (Exception ex)
        {
            IsAvailable = false;
            UnavailableReason = ex.Message;
        }
    }

    public async Task DisposeAsync()
    {
        if (_mysql is not null)
        {
            await _mysql.DisposeAsync();
        }
    }
}
