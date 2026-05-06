using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.CancelarNFe;

public sealed class CancelarNFeCommandValidation : AbstractValidator<CancelarNFeCommand>
{
    public CancelarNFeCommandValidation()
    {
        RuleFor(x => x.NFeId).NotEmpty();
        RuleFor(x => x.Justificativa).NotEmpty()
            .MinimumLength(15).WithMessage("Justificativa exige no mínimo 15 caracteres (regra SEFAZ).")
            .MaximumLength(255);
    }
}
