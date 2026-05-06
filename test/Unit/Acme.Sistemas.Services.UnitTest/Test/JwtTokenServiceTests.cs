using Acme.Sistemas.Core.Security;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class JwtTokenServiceTests
{
    private static JwtTokenService CreateService() =>
        new(Options.Create(new JwtOptions
        {
            Issuer = "atena.test",
            Audience = "atena.test.clients",
            SigningKey = "this-is-a-very-long-signing-key-for-tests-only-32-bytes-or-more!!",
            AccessTokenMinutes = 15,
            RefreshTokenDays = 7
        }));

    [Fact]
    public void Issue_DeveGerarTokensValidos()
    {
        var sut = CreateService();
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var pair = sut.Issue(tenantId, userId, "admin@atena.com", new[] { "vendas:criar", "vendas:ler" });

        pair.AccessToken.Should().NotBeNullOrEmpty();
        pair.RefreshToken.Should().NotBeNullOrEmpty();
        pair.AccessExpiresAt.Should().BeAfter(DateTime.UtcNow);
        pair.RefreshExpiresAt.Should().BeAfter(pair.AccessExpiresAt);
    }

    [Fact]
    public void TryValidate_DeveValidarTokenRecemEmitido()
    {
        var sut = CreateService();
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var pair = sut.Issue(tenantId, userId, "user@atena.com", Array.Empty<string>());

        var ok = sut.TryValidate(pair.AccessToken, out var jti, out var t, out var u);

        ok.Should().BeTrue();
        jti.Should().Be(pair.AccessJti);
        t.Should().Be(tenantId);
        u.Should().Be(userId);
    }

    [Fact]
    public void TryValidate_DeveRejeitarTokenAdulterado()
    {
        var sut = CreateService();
        var pair = sut.Issue(Guid.NewGuid(), Guid.NewGuid(), "x@y.com", Array.Empty<string>());

        var adulterado = pair.AccessToken[..^4] + "AAAA";
        var ok = sut.TryValidate(adulterado, out _, out _, out _);

        ok.Should().BeFalse();
    }

    [Fact]
    public void HashRefreshToken_DeveSerDeterministicoESha512()
    {
        var token = "some-random-refresh-token-value";

        var h1 = JwtTokenService.HashRefreshToken(token);
        var h2 = JwtTokenService.HashRefreshToken(token);

        h1.Should().Be(h2);
        h1.Length.Should().Be(128);
    }
}
