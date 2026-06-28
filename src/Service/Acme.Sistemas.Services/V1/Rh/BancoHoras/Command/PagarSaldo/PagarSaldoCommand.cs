using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

public sealed record PagarSaldoCommand(
    Guid FuncionarioId,
    string Competencia,
    int Minutos) : IRequest<ResponseDefault<PagarSaldoCommandResult>>;
