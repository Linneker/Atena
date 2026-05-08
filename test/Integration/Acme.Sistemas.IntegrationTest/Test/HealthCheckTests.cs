using System.Net;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

public class HealthCheckTests : IntegrationTestBase
{
    public HealthCheckTests(DockerEnvironment docker) : base(docker) { }

    [SkippableFact]
    public async Task Health_DeveRetornarOk()
    {
        Skip.IfNot(Docker.IsAvailable,
            $"Docker indisponível: {Docker.UnavailableReason}");

        var response = await Client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
