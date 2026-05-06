using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirCCe;

public sealed class EmitirCCeCommandValidation : AbstractValidator<EmitirCCeCommand>
{
    public EmitirCCeCommandValidation()
    {
        RuleFor(x => x.NFeId).NotEmpty();
        RuleFor(x => x.Correcao).NotEmpty()
            .MinimumLength(15).WithMessage("Correção exige no mínimo 15 caracteres (regra SEFAZ).")
            .MaximumLength(1000);
        RuleFor(x => x.Sequencia).InclusiveBetween(1, 20);
    }
}
