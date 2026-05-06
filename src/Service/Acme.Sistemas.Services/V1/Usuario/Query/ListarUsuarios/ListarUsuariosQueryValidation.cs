using FluentValidation;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

public sealed class ListarUsuariosQueryValidation : AbstractValidator<ListarUsuariosQuery>
{
    public ListarUsuariosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
