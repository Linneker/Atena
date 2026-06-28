using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.VincularBeneficio;

public sealed record VincularBeneficioCommand(
    Guid FuncionarioId,
    Guid BeneficioCatalogoId,
    decimal? Valor,
    decimal? DescontoFuncionarioPct,
    DateOnly VigenciaInicio,
    string? Observacao)
    : IRequest<ResponseDefault<VincularBeneficioCommandResult>>;
