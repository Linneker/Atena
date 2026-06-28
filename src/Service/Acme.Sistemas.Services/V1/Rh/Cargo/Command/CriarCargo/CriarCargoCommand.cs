using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.CriarCargo;

public sealed record CriarCargoCommand(
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido) : IRequest<ResponseDefault<CriarCargoCommandResult>>;
