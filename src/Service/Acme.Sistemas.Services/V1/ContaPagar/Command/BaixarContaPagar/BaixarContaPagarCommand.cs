using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;

public sealed record BaixarContaPagarCommand(
    Guid Id,
    decimal ValorPago,
    DateTime DataPagamento,
    FormaPagamento FormaPagamento,
    string? Observacao) : IRequest<ResponseDefault<BaixarContaPagarCommandResult>>;

