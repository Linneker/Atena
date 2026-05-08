using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

public sealed record BaixarDespesaCommandResult(
    Guid Id,
    StatusPagamento StatusPagamento,
    decimal ValorPago,
    DateTime DataPagamento);
