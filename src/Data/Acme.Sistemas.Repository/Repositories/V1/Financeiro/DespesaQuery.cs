namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

internal static class DespesaQuery
{
    public const string Cols = @"id, tenant_id, nome, descricao, categoria, valor, despesa_fixa,
        data_vencimento, competencia_id, centro_de_custo_id, fornecedor_id, origem_despesa_id,
        status_pagamento, valor_pago, data_pagamento, forma_pagamento, observacao_pagamento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public const string Insert = @"
        INSERT INTO despesas
        (id, tenant_id, nome, descricao, categoria, valor, despesa_fixa,
         data_vencimento, competencia_id, centro_de_custo_id, fornecedor_id, origem_despesa_id,
         status_pagamento, created_at, created_by)
        VALUES
        (@id, @tenant_id, @nome, @descricao, @categoria, @valor, @despesa_fixa,
         @data_vencimento, @competencia_id, @centro_de_custo_id, @fornecedor_id, @origem_despesa_id,
         @status_pagamento, @created_at, @created_by)";

    public const string Update = @"
        UPDATE despesas SET
            nome = @nome, descricao = @descricao, categoria = @categoria,
            valor = @valor, despesa_fixa = @despesa_fixa,
            data_vencimento = @data_vencimento,
            competencia_id = @competencia_id,
            centro_de_custo_id = @centro_de_custo_id,
            fornecedor_id = @fornecedor_id,
            updated_at = @updated_at, updated_by = @updated_by
        WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL";

    public const string Baixar = @"
        UPDATE despesas SET
            status_pagamento = @status_pagamento,
            valor_pago = @valor_pago,
            data_pagamento = @data_pagamento,
            forma_pagamento = @forma_pagamento,
            observacao_pagamento = @observacao_pagamento,
            updated_at = @updated_at, updated_by = @updated_by
        WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL";
}
