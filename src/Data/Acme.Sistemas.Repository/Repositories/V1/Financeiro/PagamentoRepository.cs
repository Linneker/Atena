using System.Data;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
{
    public PagamentoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "pagamentos";
    protected override Func<IDataRecord, Pagamento> Map => MapEntity;

    private const string Cols = @"id, tenant_id, despesa_id, divida_id, conta_pagar_id, valor,
        data_pagamento, forma_pagamento, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Pagamento p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO pagamentos
            (id, tenant_id, despesa_id, divida_id, conta_pagar_id, valor,
             data_pagamento, forma_pagamento, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @despesa_id, @divida_id, @conta_pagar_id, @valor,
             @data_pagamento, @forma, @observacao,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@despesa_id"] = p.DespesaId,
                ["@divida_id"] = p.DividaId,
                ["@conta_pagar_id"] = p.ContaPagarId,
                ["@valor"] = p.Valor,
                ["@data_pagamento"] = p.DataPagamento,
                ["@forma"] = (int)p.FormaPagamento,
                ["@observacao"] = p.Observacao,
                ["@created_at"] = p.CreatedAt,
                ["@created_by"] = p.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(Pagamento entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Pagamentos são imutáveis após registro.");

    public Task<IReadOnlyList<Pagamento>> ListByContaPagarAsync(Guid contaPagarId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM pagamentos WHERE tenant_id = @tenantId AND conta_pagar_id = @id AND deleted_at IS NULL ORDER BY data_pagamento DESC",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@id"] = contaPagarId },
            cancellationToken);

    public Task<IReadOnlyList<Pagamento>> ListByDividaAsync(Guid dividaId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM pagamentos WHERE tenant_id = @tenantId AND divida_id = @id AND deleted_at IS NULL ORDER BY data_pagamento DESC",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@id"] = dividaId },
            cancellationToken);

    private static Pagamento MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        DespesaId = r.GetValueOrDefault<Guid?>("despesa_id"),
        DividaId = r.GetValueOrDefault<Guid?>("divida_id"),
        ContaPagarId = r.GetValueOrDefault<Guid?>("conta_pagar_id"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        DataPagamento = r.GetValueOrDefault<DateTime>("data_pagamento"),
        FormaPagamento = (FormaPagamento)r.GetValueOrDefault<int>("forma_pagamento"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
