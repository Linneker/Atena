namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.AlterarDespesa;

public sealed record AlterarDespesaRequest(
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool DespesaFixa,
    DateTime DataVencimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? FornecedorId);
