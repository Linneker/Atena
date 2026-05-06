using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Users;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Autenticacao.Command.Login;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class LoginCommandHandlerTests
{
    private static (LoginCommandHandler sut, Mock<IUsuarioRepository> users, Mock<IRolePermissionRepository> rps,
                     Mock<IRefreshTokenRepository> refresh) Build()
    {
        var users = new Mock<IUsuarioRepository>();
        var rps = new Mock<IRolePermissionRepository>();
        var refresh = new Mock<IRefreshTokenRepository>();
        var jwt = new JwtTokenService(Options.Create(new JwtOptions
        {
            Issuer = "test", Audience = "test",
            SigningKey = "this-is-a-very-long-key-for-testing-only-32-bytes-min!",
            AccessTokenMinutes = 15, RefreshTokenDays = 7
        }));
        return (new LoginCommandHandler(users.Object, rps.Object, refresh.Object, jwt), users, rps, refresh);
    }

    [Fact]
    public async Task Login_ComCredenciaisCorretas_RetornaTokens()
    {
        var (sut, users, rps, _) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            Email = "admin@atena.com",
            NomeCompleto = "Admin",
            PasswordHash = PasswordHelper.Hash("Atena@2026"),
            Status = StatusAtivo.Ativo
        };
        users.Setup(u => u.GetByEmailAcrossTenantsAsync("admin@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);
        rps.Setup(r => r.GetCodigosByUserAsync(user.Id, It.IsAny<CancellationToken>()))
           .ReturnsAsync(new[] { "tenant:ler" });

        var result = await sut.Handle(new LoginCommand("admin@atena.com", "Atena@2026", null, null), default);

        result.IsSuccess.Should().BeTrue();
        result.Content!.AccessToken.Should().NotBeNullOrEmpty();
        result.Content.UserId.Should().Be(user.Id);
        result.Content.Permissions.Should().Contain("tenant:ler");
    }

    [Fact]
    public async Task Login_AposCincoTentativas_BloqueiaConta()
    {
        var (sut, users, _, _) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "user@atena.com",
            PasswordHash = PasswordHelper.Hash("CorretaSenha@1"),
            Status = StatusAtivo.Ativo,
            FailedLoginAttempts = 4
        };
        users.Setup(u => u.GetByEmailAcrossTenantsAsync("user@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);

        DateTime? capturedLock = null;
        users.Setup(u => u.UpdateLoginStatusAsync(user.Id, It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
             .Callback<Guid, int, DateTime?, DateTime?, CancellationToken>((_, _, locked, _, _) => capturedLock = locked)
             .Returns(Task.CompletedTask);

        var result = await sut.Handle(new LoginCommand("user@atena.com", "errada", null, null), default);

        result.IsSuccess.Should().BeFalse();
        capturedLock.Should().NotBeNull();
        capturedLock!.Value.Should().BeAfter(DateTime.UtcNow.AddMinutes(10));
    }

    [Fact]
    public async Task Login_ContaBloqueada_RecusaImediatamente()
    {
        var (sut, users, _, _) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "locked@atena.com",
            PasswordHash = PasswordHelper.Hash("Senha@123"),
            Status = StatusAtivo.Ativo,
            LockedUntil = DateTime.UtcNow.AddMinutes(10)
        };
        users.Setup(u => u.GetByEmailAcrossTenantsAsync("locked@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);

        var result = await sut.Handle(new LoginCommand("locked@atena.com", "Senha@123", null, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(400);
    }
}
