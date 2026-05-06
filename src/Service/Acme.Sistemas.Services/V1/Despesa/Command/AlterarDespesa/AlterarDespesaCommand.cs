using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

public sealed record AlterarDespesaCommand(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool DespesaFixa,
    DateTime DataVencimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? FornecedorId) : IRequest<ResponseDefault<AlterarDespesaCommandResult>>;

public sealed record AlterarDespesaCommandResult(Guid Id);
