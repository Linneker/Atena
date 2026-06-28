using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioContrato;

public sealed record AlterarFuncionarioContratoCommand(
    Guid Id,
    Guid? CargoId,
    Guid? LotacaoId,
    Guid? DepartamentoId,
    Guid? CentroDeCustoId,
    TipoContrato? TipoContrato,
    RegimeRemuneracao? RegimeRemuneracao,
    string? CodigoMatricula,
    DateOnly? DataDemissao,
    StatusAtivo Status)
    : IRequest<ResponseDefault<AlterarFuncionarioContratoCommandResult>>;
