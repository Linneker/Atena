using System.Net;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

public class HealthCheckTests : IntegrationTestBase
{
    public HealthCheckTests(DockerEnvironment docker) : base(docker) { }

    [Fact]
    public async Task Health_DeveRetornarOk()
    {
        var response = await Client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
