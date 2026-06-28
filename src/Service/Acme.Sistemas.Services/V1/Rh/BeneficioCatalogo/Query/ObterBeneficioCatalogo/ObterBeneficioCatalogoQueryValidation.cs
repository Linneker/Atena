using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

public sealed class ObterBeneficioCatalogoQueryValidation : AbstractValidator<ObterBeneficioCatalogoQuery>
{
    public ObterBeneficioCatalogoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
