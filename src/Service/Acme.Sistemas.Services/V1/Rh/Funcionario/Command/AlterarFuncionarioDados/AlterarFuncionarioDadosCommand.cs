using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioDados;

public sealed record AlterarFuncionarioDadosCommand(
    Guid Id,
    string NomeCompleto,
    string? Email,
    string? Telefone,
    string? Rg,
    string? RgOrgao,
    string? RgUf,
    EstadoCivil? EstadoCivil,
    string? Naturalidade,
    string? Nacionalidade,
    EnderecoFuncionario? Endereco,
    ContaBancariaFuncionario? ContaBancaria)
    : IRequest<ResponseDefault<AlterarFuncionarioDadosCommandResult>>;
