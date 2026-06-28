using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

public sealed record FichaDadosPessoais(
    Guid Id,
    string NomeCompleto,
    string Cpf,
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

public sealed record FichaContrato(
    DateTime? DataAdmissao,
    DateTime? DataDemissao,
    Guid? CargoId,
    Guid? LotacaoId,
    Guid? DepartamentoId,
    Guid? CentroDeCustoId,
    TipoContrato? TipoContrato,
    RegimeRemuneracao? RegimeRemuneracao,
    string? CodigoMatricula,
    string? Pis,
    string? Ctps,
    string? CtpsSerie,
    string? CtpsUf,
    StatusAtivo Status);

public sealed record FichaSalarioItem(
    Guid Id,
    decimal Valor,
    DateOnly VigenciaInicio,
    DateOnly? VigenciaFim,
    MotivoSalario Motivo,
    string? Observacao);

public sealed record FichaBeneficioItem(
    Guid Id,
    Guid BeneficioCatalogoId,
    decimal? Valor,
    decimal? DescontoFuncionarioPct,
    DateOnly VigenciaInicio,
    DateOnly? VigenciaFim);

public sealed record FichaDependenteItem(
    Guid Id,
    string NomeCompleto,
    string? Cpf,
    DateOnly DataNascimento,
    TipoDependente Tipo,
    bool Irrf,
    bool SalarioFamilia,
    decimal? PensaoAlimenticiaPct);

public sealed record FichaEscalaItem(
    Guid Id,
    Guid JornadaId,
    DateOnly VigenciaInicio,
    DateOnly? VigenciaFim,
    string? Observacao);

public sealed record ObterFichaCompletaQueryResult(
    FichaDadosPessoais DadosPessoais,
    FichaContrato Contrato,
    decimal? SalarioVigente,
    IReadOnlyList<FichaSalarioItem> HistoricoSalarial,
    IReadOnlyList<FichaBeneficioItem> Beneficios,
    IReadOnlyList<FichaDependenteItem> Dependentes,
    IReadOnlyList<FichaEscalaItem> Escalas);
