using System.Security.Claims;
using Acme.Sistemas.Atena.Api.Config.Security;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class HttpTenantContextAccessorTests
{
    [Trait("Solucao", "Api")]
    [Trait("Acao", "TenantContext")]
    [Fact(DisplayName = "Dado JWT com claims de tenant, user e permissões, quando acessor consultado, então expõe IsAuthenticated, TenantId, UserId e lista de Permissions")]
    public void DeveExtrairTenantIdEPermissoesDoJwt()
    {
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(TenantClaims.TenantId, tenantId.ToString()),
            new Claim(TenantClaims.UserId, userId.ToString()),
            new Claim(TenantClaims.Permissions, "vendas:criar"),
            new Claim(TenantClaims.Permissions, "vendas:ler")
        }, "Bearer");
        var http = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(http);

        var sut = new HttpTenantContextAccessor(accessor.Object);

        sut.IsAuthenticated.Should().BeTrue();
        sut.TenantId.Should().Be(tenantId);
        sut.UserId.Should().Be(userId);
        sut.Permissions.Should().BeEquivalentTo(new[] { "vendas:criar", "vendas:ler" });
    }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "TenantContext")]
    [Fact(DisplayName = "Dado HttpContext sem autenticação, quando acessor consultado, então IsAuthenticated=false, TenantId=Empty e Permissions vazia")]
    public void SemAutenticacaoRetornaContextoVazio()
    {
        var http = new DefaultHttpContext();
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(http);

        var sut = new HttpTenantContextAccessor(accessor.Object);

        sut.IsAuthenticated.Should().BeFalse();
        sut.TenantId.Should().Be(Guid.Empty);
        sut.UserId.Should().BeNull();
        sut.Permissions.Should().BeEmpty();
    }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "TenantContext")]
    [Fact(DisplayName = "Dado claim de tenant com valor não-Guid, quando TenantId consultado, então retorna Guid.Empty sem lançar")]
    public void TenantClaimInvalidoResultaEmGuidEmpty()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(TenantClaims.TenantId, "not-a-guid")
        }, "Bearer");
        var http = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(http);

        var sut = new HttpTenantContextAccessor(accessor.Object);

        sut.TenantId.Should().Be(Guid.Empty);
    }
}
