using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;

public sealed record AlterarCentroDeCustoCommand(
    Guid Id,
    string Nome,
    string? Descricao,
    Guid? ResponsavelId,
    bool Ativo) : IRequest<ResponseDefault<AlterarCentroDeCustoCommandResult>>;

