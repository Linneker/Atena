using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CriarFuncionarioCompleto;

public sealed record DependenteRequestDto(
    string NomeCompleto,
    string? Cpf,
    DateOnly DataNascimento,
    TipoDependente Tipo,
    bool Irrf = false,
    bool SalarioFamilia = false,
    decimal? PensaoAlimenticiaPct = null);

public sealed record BeneficioRequestDto(
    Guid BeneficioCatalogoId,
    decimal? Valor,
    decimal? DescontoFuncionarioPct,
    DateOnly VigenciaInicio);

public sealed record CriarFuncionarioCompletoRequest(
    string NomeCompleto,
    string Cpf,
    string? Email,
    string? Telefone,
    DateOnly? DataNascimento,
    EstadoCivil? EstadoCivil,
    string? Naturalidade,
    string? Nacionalidade,
    string? Rg,
    string? RgOrgao,
    string? RgUf,
    EnderecoFuncionario? Endereco,
    DateOnly DataAdmissao,
    Guid? CargoId,
    Guid? LotacaoId,
    Guid? DepartamentoId,
    Guid? CentroDeCustoId,
    TipoContrato TipoContrato,
    RegimeRemuneracao RegimeRemuneracao,
    string? CodigoMatricula,
    string? Pis,
    string? Ctps,
    string? CtpsSerie,
    string? CtpsUf,
    decimal SalarioInicial,
    ContaBancariaFuncionario? ContaBancaria,
    Guid? JornadaId,
    IReadOnlyList<BeneficioRequestDto>? Beneficios = null,
    IReadOnlyList<DependenteRequestDto>? Dependentes = null);
