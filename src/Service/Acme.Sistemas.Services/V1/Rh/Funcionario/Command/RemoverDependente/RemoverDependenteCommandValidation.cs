using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverDependente;

public sealed class RemoverDependenteCommandValidation : AbstractValidator<RemoverDependenteCommand>
{
    public RemoverDependenteCommandValidation()
    {
        RuleFor(x => x.DependenteId).NotEmpty();
    }
}
