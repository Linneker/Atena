using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.RemoverBeneficioCatalogo;

public sealed class RemoverBeneficioCatalogoCommandValidation : AbstractValidator<RemoverBeneficioCatalogoCommand>
{
    public RemoverBeneficioCatalogoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
