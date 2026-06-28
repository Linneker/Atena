using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.AlterarDepartamento;

public sealed record AlterarDepartamentoCommand(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo) : IRequest<ResponseDefault<AlterarDepartamentoCommandResult>>;
