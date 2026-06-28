using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

public sealed class ObterDepartamentoQueryValidation : AbstractValidator<ObterDepartamentoQuery>
{
    public ObterDepartamentoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
