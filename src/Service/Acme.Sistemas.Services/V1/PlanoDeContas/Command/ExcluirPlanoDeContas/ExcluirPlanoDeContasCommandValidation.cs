using FluentValidation;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;

public sealed class ExcluirPlanoDeContasCommandValidation : AbstractValidator<ExcluirPlanoDeContasCommand>
{
    public ExcluirPlanoDeContasCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
