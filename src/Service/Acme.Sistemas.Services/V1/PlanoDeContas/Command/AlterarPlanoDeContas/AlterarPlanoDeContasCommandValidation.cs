using FluentValidation;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;

public sealed class AlterarPlanoDeContasCommandValidation : AbstractValidator<AlterarPlanoDeContasCommand>
{
    public AlterarPlanoDeContasCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
    }
}
