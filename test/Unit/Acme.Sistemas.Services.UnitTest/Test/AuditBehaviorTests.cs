using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Behaviors;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class AuditBehaviorTests
{
    public sealed record CriarFooCommand(Guid Id, string Nome) : IRequest<string>, IAuditable
    {
        public string Recurso => "Foo";
        public string Acao => "Criar";
    }

    public sealed record ListarFooQuery(string Filtro) : IRequest<string>;

    [Fact]
    public async Task NaoAuditable_NaoPersisteLog()
    {
        var audit = new Mock<IAuditLogRepository>();
        var tenant = new Mock<ITenantContext>();
        var sut = new AuditBehavior<ListarFooQuery, string>(
            audit.Object, tenant.Object,
            NullLogger<AuditBehavior<ListarFooQuery, string>>.Instance);

        await sut.Handle(new ListarFooQuery("x"), () => Task.FromResult("ok"), default);

        audit.Verify(a => a.AddAsync(It.IsAny<AuditLog>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Auditable_PersisteLogComRecursoAcaoEId()
    {
        var audit = new Mock<IAuditLogRepository>();
        var tenant = new Mock<ITenantContext>();
        tenant.SetupGet(t => t.TenantId).Returns(Guid.NewGuid());
        tenant.SetupGet(t => t.UserId).Returns(Guid.NewGuid());

        AuditLog? capturado = null;
        audit.Setup(a => a.AddAsync(It.IsAny<AuditLog>(), It.IsAny<CancellationToken>()))
             .Callback<AuditLog, CancellationToken>((log, _) => capturado = log)
             .Returns(Task.CompletedTask);

        var sut = new AuditBehavior<CriarFooCommand, string>(
            audit.Object, tenant.Object,
            NullLogger<AuditBehavior<CriarFooCommand, string>>.Instance);

        var id = Guid.NewGuid();
        await sut.Handle(new CriarFooCommand(id, "n"), () => Task.FromResult("ok"), default);

        capturado.Should().NotBeNull();
        capturado!.EntidadeNome.Should().Be("Foo");
        capturado.Operacao.Should().Be(OperacaoAuditoria.Criar);
        capturado.EntidadeId.Should().Be(id);
    }

    [Fact]
    public async Task FalhaAoPersistirNaoQuebraFluxo()
    {
        var audit = new Mock<IAuditLogRepository>();
        audit.Setup(a => a.AddAsync(It.IsAny<AuditLog>(), It.IsAny<CancellationToken>()))
             .ThrowsAsync(new InvalidOperationException("db down"));
        var tenant = new Mock<ITenantContext>();

        var sut = new AuditBehavior<CriarFooCommand, string>(
            audit.Object, tenant.Object,
            NullLogger<AuditBehavior<CriarFooCommand, string>>.Instance);

        var resultado = await sut.Handle(new CriarFooCommand(Guid.NewGuid(), "n"),
            () => Task.FromResult("ok"), default);

        resultado.Should().Be("ok");
    }
}
