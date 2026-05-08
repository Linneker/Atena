using System.Net;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

public class HealthCheckTests : IntegrationTestBase
{
    public HealthCheckTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "HealthCheck")]
    [SkippableFact(DisplayName = "Dado a aplicação de pé, quando GET /health, então retorna 200 OK")]
    public async Task Health_DeveRetornarOk()
    {
        Skip.IfNot(Docker.IsAvailable,
            $"Docker indisponível: {Docker.UnavailableReason}");

        var response = await Client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
