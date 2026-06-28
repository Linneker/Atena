using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CadastrarDependente;

public sealed record CadastrarDependenteCommand(
    Guid FuncionarioId,
    string NomeCompleto,
    string? Cpf,
    DateOnly DataNascimento,
    TipoDependente Tipo,
    bool Irrf,
    bool SalarioFamilia,
    decimal? PensaoAlimenticiaPct)
    : IRequest<ResponseDefault<CadastrarDependenteCommandResult>>;
