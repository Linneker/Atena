namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

internal static class ReceitaQuery
{
    public const string Cols = @"id, tenant_id, nome, descricao, categoria, valor, receita_fixa,
        data_prevista_recebimento, competencia_id, centro_de_custo_id, cliente_id, origem_venda_id,
        status_recebimento, valor_recebido, data_recebimento, forma_pagamento, observacao_recebimento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public const string Insert = @"
        INSERT INTO receitas
        (id, tenant_id, nome, descricao, categoria, valor, receita_fixa,
         data_prevista_recebimento, competencia_id, centro_de_custo_id, cliente_id, origem_venda_id,
         status_recebimento, created_at, created_by)
        VALUES
        (@id, @tenant_id, @nome, @descricao, @categoria, @valor, @receita_fixa,
         @data_prevista_recebimento, @competencia_id, @centro_de_custo_id, @cliente_id, @origem_venda_id,
         @status_recebimento, @created_at, @created_by)";

    public const string Update = @"
        UPDATE receitas SET
            nome = @nome, descricao = @descricao, categoria = @categoria,
            valor = @valor, receita_fixa = @receita_fixa,
            data_prevista_recebimento = @data_prevista_recebimento,
            competencia_id = @competencia_id,
            centro_de_custo_id = @centro_de_custo_id,
            cliente_id = @cliente_id,
            origem_venda_id = @origem_venda_id,
            updated_at = @updated_at, updated_by = @updated_by
        WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL";

    public const string Receber = @"
        UPDATE receitas SET
            status_recebimento = @status_recebimento,
            valor_recebido = @valor_recebido,
            data_recebimento = @data_recebimento,
            forma_pagamento = @forma_pagamento,
            observacao_recebimento = @observacao_recebimento,
            updated_at = @updated_at, updated_by = @updated_by
        WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL";
}
