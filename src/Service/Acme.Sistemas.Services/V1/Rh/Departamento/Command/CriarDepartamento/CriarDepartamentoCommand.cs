using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.CriarDepartamento;

public sealed record CriarDepartamentoCommand(
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId) : IRequest<ResponseDefault<CriarDepartamentoCommandResult>>;
