namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.CriarReceita;

public sealed record CriarReceitaRequest(
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool ReceitaFixa,
    DateTime DataPrevistaRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? ClienteId,
    Guid? OrigemVendaId);
