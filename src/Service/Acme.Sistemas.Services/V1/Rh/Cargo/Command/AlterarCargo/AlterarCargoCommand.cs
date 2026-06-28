using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.AlterarCargo;

public sealed record AlterarCargoCommand(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo) : IRequest<ResponseDefault<AlterarCargoCommandResult>>;
