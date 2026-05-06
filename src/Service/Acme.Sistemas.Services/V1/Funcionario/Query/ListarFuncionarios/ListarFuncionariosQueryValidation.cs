using FluentValidation;

namespace Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

public sealed class ListarFuncionariosQueryValidation : AbstractValidator<ListarFuncionariosQuery>
{
    public ListarFuncionariosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 500);
    }
}
