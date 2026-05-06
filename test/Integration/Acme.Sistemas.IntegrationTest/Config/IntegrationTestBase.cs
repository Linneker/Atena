using Xunit;

namespace Acme.Sistemas.IntegrationTest.Config;

[Collection("Docker")]
public abstract class IntegrationTestBase : IClassFixture<DockerEnvironment>, IAsyncLifetime
{
    protected DockerEnvironment Docker { get; }
    protected IntegrationWebApplicationFactory Factory { get; }
    protected HttpClient Client { get; private set; } = null!;

    protected IntegrationTestBase(DockerEnvironment docker)
    {
        Docker = docker;
        Factory = new IntegrationWebApplicationFactory();
    }

    public virtual Task InitializeAsync()
    {
        Factory.ConnectionString = Docker.MySqlConnectionString;
        Client = Factory.CreateClient();
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        Client.Dispose();
        return Factory.DisposeAsync().AsTask();
    }
}
