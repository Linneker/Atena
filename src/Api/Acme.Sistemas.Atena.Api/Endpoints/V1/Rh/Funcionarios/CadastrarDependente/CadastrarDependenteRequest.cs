using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CadastrarDependente;

public sealed record CadastrarDependenteRequest(
    Guid FuncionarioId,
    string NomeCompleto,
    string? Cpf,
    DateOnly DataNascimento,
    TipoDependente Tipo,
    bool Irrf,
    bool SalarioFamilia,
    decimal? PensaoAlimenticiaPct);
