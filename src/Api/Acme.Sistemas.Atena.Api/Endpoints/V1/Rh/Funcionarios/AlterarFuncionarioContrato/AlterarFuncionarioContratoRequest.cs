using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AlterarFuncionarioContrato;

public sealed record AlterarFuncionarioContratoRequest(
    Guid Id,
    Guid? CargoId,
    Guid? LotacaoId,
    Guid? DepartamentoId,
    Guid? CentroDeCustoId,
    TipoContrato? TipoContrato,
    RegimeRemuneracao? RegimeRemuneracao,
    string? CodigoMatricula,
    DateOnly? DataDemissao,
    StatusAtivo Status);
