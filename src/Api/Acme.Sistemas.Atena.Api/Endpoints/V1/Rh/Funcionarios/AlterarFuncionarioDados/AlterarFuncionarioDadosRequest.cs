using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AlterarFuncionarioDados;

public sealed record AlterarFuncionarioDadosRequest(
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
    ContaBancariaFuncionario? ContaBancaria);
