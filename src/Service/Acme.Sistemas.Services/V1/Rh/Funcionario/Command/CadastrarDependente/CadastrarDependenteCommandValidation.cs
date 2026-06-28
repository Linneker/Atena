using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CadastrarDependente;

public sealed class CadastrarDependenteCommandValidation : AbstractValidator<CadastrarDependenteCommand>
{
    public CadastrarDependenteCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cpf).Matches(@"^\d{11}$").When(x => !string.IsNullOrWhiteSpace(x.Cpf));
        RuleFor(x => x.PensaoAlimenticiaPct).InclusiveBetween(0, 100)
            .When(x => x.PensaoAlimenticiaPct.HasValue);
    }
}
