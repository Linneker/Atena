using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

public sealed class ListarBeneficiosCatalogoQueryValidation : AbstractValidator<ListarBeneficiosCatalogoQuery>
{
    public ListarBeneficiosCatalogoQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
