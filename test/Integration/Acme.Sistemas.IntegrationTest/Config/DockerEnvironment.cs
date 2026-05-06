using Testcontainers.MySql;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Config;

public sealed class DockerEnvironment : IAsyncLifetime
{
    private readonly MySqlContainer _mysql = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("atena_test")
        .WithUsername("root")
        .WithPassword("root")
        .Build();

    public string MySqlConnectionString => _mysql.GetConnectionString();

    public async Task InitializeAsync() => await _mysql.StartAsync();

    public async Task DisposeAsync() => await _mysql.DisposeAsync();
}
