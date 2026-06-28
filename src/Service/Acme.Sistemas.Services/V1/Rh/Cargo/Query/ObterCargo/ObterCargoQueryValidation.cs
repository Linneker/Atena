using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

public sealed class ObterCargoQueryValidation : AbstractValidator<ObterCargoQuery>
{
    public ObterCargoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
