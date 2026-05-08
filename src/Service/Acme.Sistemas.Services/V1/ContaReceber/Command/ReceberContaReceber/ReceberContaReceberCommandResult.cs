using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

public sealed record ReceberContaReceberCommandResult(
    Guid Id, StatusConta Status, decimal ValorRecebido, decimal Saldo);
