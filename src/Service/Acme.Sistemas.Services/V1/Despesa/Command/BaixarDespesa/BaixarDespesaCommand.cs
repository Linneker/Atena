using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

public sealed record BaixarDespesaCommand(
    Guid Id,
    decimal ValorPago,
    DateTime DataPagamento,
    FormaPagamento FormaPagamento,
    string? Observacao) : IRequest<ResponseDefault<BaixarDespesaCommandResult>>;

