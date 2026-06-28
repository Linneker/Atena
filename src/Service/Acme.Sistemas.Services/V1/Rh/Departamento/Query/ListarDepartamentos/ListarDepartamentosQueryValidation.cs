using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

public sealed class ListarDepartamentosQueryValidation : AbstractValidator<ListarDepartamentosQuery>
{
    public ListarDepartamentosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
