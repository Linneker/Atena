using System.Data;
using Acme.Sistemas.Domain.Entities;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class BaseRepositoryTenantFilterTests
{
    private sealed class TestEntity : BaseEntity { }

    private sealed class TestRepository : BaseRepository<TestEntity>
    {
        public TestRepository(IDataConfiguration db, ITenantContext ctx) : base(db, ctx) { }
        protected override string TableName => "test_table";
        protected override Func<IDataRecord, TestEntity> Map => _ => new TestEntity();
        public override Task AddAsync(TestEntity e, CancellationToken c = default) => Task.CompletedTask;
        public override Task UpdateAsync(TestEntity e, CancellationToken c = default) => Task.CompletedTask;
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "TenantFilter")]
    [Fact(DisplayName = "Dado um BaseRepository, quando GetByIdAsync, então a SQL recebe parâmetros @tenantId e @id do contexto")]
    public async Task GetByIdAsync_DeveAplicarFiltroDeTenantId()
    {
        var tenantId = Guid.NewGuid();
        var registroId = Guid.NewGuid();

        var ctx = new Mock<ITenantContext>();
        ctx.SetupGet(c => c.TenantId).Returns(tenantId);

        var db = new Mock<IDataConfiguration>();
        IDictionary<string, object?>? capturedParams = null;
        db.Setup(d => d.QueryFirstOrDefaultAsync(
                It.IsAny<string>(),
                It.IsAny<Func<IDataRecord, TestEntity>>(),
                It.IsAny<IDictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Func<IDataRecord, TestEntity>, IDictionary<string, object?>?, CancellationToken>(
                (_, _, p, _) => capturedParams = p)
            .ReturnsAsync((TestEntity?)null);

        var repo = new TestRepository(db.Object, ctx.Object);
        await repo.GetByIdAsync(registroId);

        capturedParams.Should().NotBeNull();
        capturedParams!["@tenantId"].Should().Be(tenantId);
        capturedParams["@id"].Should().Be(registroId);
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "TenantFilter")]
    [Fact(DisplayName = "Dado um BaseRepository, quando ListAsync com skip e take, então a SQL contém WHERE tenant_id = @tenantId AND deleted_at IS NULL e parâmetros de paginação")]
    public async Task ListAsync_DeveAplicarFiltroDeTenantIdEPaginacao()
    {
        var tenantId = Guid.NewGuid();
        var ctx = new Mock<ITenantContext>();
        ctx.SetupGet(c => c.TenantId).Returns(tenantId);

        var db = new Mock<IDataConfiguration>();
        IDictionary<string, object?>? capturedParams = null;
        string? capturedSql = null;
        db.Setup(d => d.QueryAsync(
                It.IsAny<string>(),
                It.IsAny<Func<IDataRecord, TestEntity>>(),
                It.IsAny<IDictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Func<IDataRecord, TestEntity>, IDictionary<string, object?>?, CancellationToken>(
                (sql, _, p, _) => { capturedSql = sql; capturedParams = p; })
            .ReturnsAsync(Array.Empty<TestEntity>());

        var repo = new TestRepository(db.Object, ctx.Object);
        await repo.ListAsync(skip: 10, take: 25);

        capturedParams!["@tenantId"].Should().Be(tenantId);
        capturedParams["@skip"].Should().Be(10);
        capturedParams["@take"].Should().Be(25);
        capturedSql.Should().Contain("tenant_id = @tenantId").And.Contain("deleted_at IS NULL");
    }
}
