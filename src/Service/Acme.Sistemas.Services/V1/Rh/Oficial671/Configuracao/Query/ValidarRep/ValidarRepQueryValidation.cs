using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

public sealed class ValidarRepQueryValidation : AbstractValidator<ValidarRepQuery>
{
    public ValidarRepQueryValidation() => RuleFor(x => x.EmpresaId).NotEmpty();
}
