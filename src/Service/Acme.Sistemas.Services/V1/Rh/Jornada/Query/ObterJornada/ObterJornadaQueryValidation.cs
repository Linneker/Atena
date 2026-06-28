using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

public sealed class ObterJornadaQueryValidation : AbstractValidator<ObterJornadaQuery>
{
    public ObterJornadaQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
