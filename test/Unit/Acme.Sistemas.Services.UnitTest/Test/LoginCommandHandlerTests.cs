using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Tenants;
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
    private const string TenantCnpj = "12345678000199";

    private static (LoginCommandHandler sut,
                    Mock<IUsuarioRepository> users,
                    Mock<ITenantRepository> tenants,
                    Mock<IRolePermissionRepository> rps,
                    Mock<IRefreshTokenRepository> refresh,
                    Tenant tenant) Build()
    {
        var users = new Mock<IUsuarioRepository>();
        var tenants = new Mock<ITenantRepository>();
        var rps = new Mock<IRolePermissionRepository>();
        var refresh = new Mock<IRefreshTokenRepository>();
        var jwt = new JwtTokenService(Options.Create(new JwtOptions
        {
            Issuer = "test", Audience = "test",
            SigningKey = "this-is-a-very-long-key-for-testing-only-32-bytes-min!",
            AccessTokenMinutes = 15, RefreshTokenDays = 7
        }));

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            RazaoSocial = "Acme",
            Cnpj = TenantCnpj,
            Plano = "FREE",
            Status = StatusAtivo.Ativo,
            FusoHorario = "America/Sao_Paulo"
        };
        tenants.Setup(t => t.GetByCnpjAsync(TenantCnpj, It.IsAny<CancellationToken>()))
               .ReturnsAsync(tenant);

        var sut = new LoginCommandHandler(users.Object, tenants.Object, rps.Object, refresh.Object, jwt);
        return (sut, users, tenants, rps, refresh, tenant);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "Login")]
    [Fact(DisplayName = "Dado credenciais corretas, quando login, então retorna access token, refresh token e permissões")]
    public async Task Login_ComCredenciaisCorretas_RetornaTokens()
    {
        var (sut, users, _, rps, _, tenant) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = "admin@atena.com",
            NomeCompleto = "Admin",
            PasswordHash = PasswordHelper.Hash("Atena@2026"),
            Status = StatusAtivo.Ativo,
            EmailConfirmedAt = DateTime.UtcNow
        };
        users.Setup(u => u.GetByEmailAsync(tenant.Id, "admin@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);
        rps.Setup(r => r.GetCodigosByUserAsync(user.Id, It.IsAny<CancellationToken>()))
           .ReturnsAsync(new[] { "tenant:ler" });

        var result = await sut.Handle(
            new LoginCommand(TenantCnpj, "admin@atena.com", "Atena@2026", null, null), default);

        result.IsSuccess.Should().BeTrue();
        result.Content!.AccessToken.Should().NotBeNullOrEmpty();
        result.Content.UserId.Should().Be(user.Id);
        result.Content.Permissions.Should().Contain("tenant:ler");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "Login")]
    [Fact(DisplayName = "Dado 4 tentativas falhas anteriores, quando login falha pela 5ª vez, então bloqueia a conta por mais de 10 minutos")]
    public async Task Login_AposCincoTentativas_BloqueiaConta()
    {
        var (sut, users, _, _, _, tenant) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = "user@atena.com",
            PasswordHash = PasswordHelper.Hash("CorretaSenha@1"),
            Status = StatusAtivo.Ativo,
            EmailConfirmedAt = DateTime.UtcNow,
            FailedLoginAttempts = 4
        };
        users.Setup(u => u.GetByEmailAsync(tenant.Id, "user@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);

        DateTime? capturedLock = null;
        users.Setup(u => u.UpdateLoginStatusAsync(user.Id, It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
             .Callback<Guid, int, DateTime?, DateTime?, CancellationToken>((_, _, locked, _, _) => capturedLock = locked)
             .Returns(Task.CompletedTask);

        var result = await sut.Handle(
            new LoginCommand(TenantCnpj, "user@atena.com", "errada", null, null), default);

        result.IsSuccess.Should().BeFalse();
        capturedLock.Should().NotBeNull();
        capturedLock!.Value.Should().BeAfter(DateTime.UtcNow.AddMinutes(10));
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "Login")]
    [Fact(DisplayName = "Dado conta bloqueada (LockedUntil futuro), quando login com senha correta, então recusa imediatamente sem validar credencial")]
    public async Task Login_ContaBloqueada_RecusaImediatamente()
    {
        var (sut, users, _, _, _, tenant) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = "locked@atena.com",
            PasswordHash = PasswordHelper.Hash("Senha@123"),
            Status = StatusAtivo.Ativo,
            EmailConfirmedAt = DateTime.UtcNow,
            LockedUntil = DateTime.UtcNow.AddMinutes(10)
        };
        users.Setup(u => u.GetByEmailAsync(tenant.Id, "locked@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);

        var result = await sut.Handle(
            new LoginCommand(TenantCnpj, "locked@atena.com", "Senha@123", null, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(400);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "Login")]
    [Fact(DisplayName = "Dado usuário com e-mail não confirmado, quando login, então retorna mensagem específica de e-mail pendente")]
    public async Task Login_EmailNaoConfirmado_RetornaErroEspecifico()
    {
        var (sut, users, _, _, _, tenant) = Build();
        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = "novo@atena.com",
            PasswordHash = PasswordHelper.Hash("Senha@123"),
            Status = StatusAtivo.PendenteConfirmacao
        };
        users.Setup(u => u.GetByEmailAsync(tenant.Id, "novo@atena.com", It.IsAny<CancellationToken>()))
             .ReturnsAsync(user);

        var result = await sut.Handle(
            new LoginCommand(TenantCnpj, "novo@atena.com", "Senha@123", null, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("confirmado");
    }
}
