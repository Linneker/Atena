namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.CriarDespesa;

public sealed record CriarDespesaRequest(
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool DespesaFixa,
    DateTime DataVencimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? FornecedorId);
