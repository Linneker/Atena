using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

public sealed record DependenteInicialDto(
    string NomeCompleto,
    string? Cpf,
    DateOnly DataNascimento,
    TipoDependente Tipo,
    bool Irrf = false,
    bool SalarioFamilia = false,
    decimal? PensaoAlimenticiaPct = null);

public sealed record BeneficioInicialDto(
    Guid BeneficioCatalogoId,
    decimal? Valor,
    decimal? DescontoFuncionarioPct,
    DateOnly VigenciaInicio);

/// <summary>
/// Criação atômica de funcionário completo: dados pessoais + contrato + salário inicial +
/// escala opcional + benefícios opcionais + dependentes opcionais. Usa transação implícita
/// (todas as inserções no mesmo connection scope; rollback pelo connection se houver falha).
/// </summary>
public sealed record CriarFuncionarioCompletoCommand(
    // Dados pessoais
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
    // Contrato
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
    // Salário inicial
    decimal SalarioInicial,
    // Conta bancária
    ContaBancariaFuncionario? ContaBancaria,
    // Escala opcional
    Guid? JornadaId,
    // Vínculos opcionais
    IReadOnlyList<BeneficioInicialDto>? Beneficios = null,
    IReadOnlyList<DependenteInicialDto>? Dependentes = null)
    : IRequest<ResponseDefault<CriarFuncionarioCompletoCommandResult>>;
