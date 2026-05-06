using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

public sealed record ReceberContaReceberCommand(
    Guid Id,
    decimal ValorRecebido,
    DateTime DataRecebimento,
    string? Observacao) : IRequest<ResponseDefault<ReceberContaReceberCommandResult>>;

public sealed record ReceberContaReceberCommandResult(
    Guid Id, StatusConta Status, decimal ValorRecebido, decimal Saldo);
