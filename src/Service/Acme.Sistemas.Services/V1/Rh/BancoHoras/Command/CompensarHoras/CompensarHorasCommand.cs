using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

public sealed record CompensarHorasCommand(
    Guid FuncionarioId,
    DateOnly Data,
    int Minutos,
    string Motivo) : IRequest<ResponseDefault<CompensarHorasCommandResult>>;
