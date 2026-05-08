using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

public sealed record AlterarReceitaCommand(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool ReceitaFixa,
    DateTime DataPrevistaRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? ClienteId,
    Guid? OrigemVendaId) : IRequest<ResponseDefault<AlterarReceitaCommandResult>>;

