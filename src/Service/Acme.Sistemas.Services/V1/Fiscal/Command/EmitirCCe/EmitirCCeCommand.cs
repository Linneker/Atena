using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirCCe;

public sealed record EmitirCCeCommand(
    Guid NFeId,
    string Correcao,
    int Sequencia = 1) : IRequest<ResponseDefault<EmitirCCeCommandResult>>;

public sealed record EmitirCCeCommandResult(
    Guid NFeId,
    int Sequencia,
    string? Protocolo);
