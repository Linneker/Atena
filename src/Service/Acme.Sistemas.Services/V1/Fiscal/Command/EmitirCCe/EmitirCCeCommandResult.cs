using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirCCe;

public sealed record EmitirCCeCommandResult(
    Guid NFeId,
    int Sequencia,
    string? Protocolo);
