using FluentValidation;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;

public sealed class CriarPlanoDeContasCommandValidation : AbstractValidator<CriarPlanoDeContasCommand>
{
    public CriarPlanoDeContasCommandValidation()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Tipo).IsInEnum();
    }
}
