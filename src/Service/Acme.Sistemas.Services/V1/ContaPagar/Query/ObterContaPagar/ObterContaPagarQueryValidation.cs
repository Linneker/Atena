using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

public sealed class ObterContaPagarQueryValidation : AbstractValidator<ObterContaPagarQuery>
{
    public ObterContaPagarQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
