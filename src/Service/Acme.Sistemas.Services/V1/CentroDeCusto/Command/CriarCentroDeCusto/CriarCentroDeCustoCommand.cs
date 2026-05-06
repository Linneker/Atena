using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;

public sealed record CriarCentroDeCustoCommand(
    string Codigo,
    string Nome,
    string? Descricao,
    Guid? ResponsavelId) : IRequest<ResponseDefault<CriarCentroDeCustoCommandResult>>;

public sealed record CriarCentroDeCustoCommandResult(Guid Id, string Codigo, string Nome);
