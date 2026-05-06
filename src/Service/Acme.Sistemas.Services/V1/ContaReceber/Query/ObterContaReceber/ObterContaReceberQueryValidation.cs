using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

public sealed class ObterContaReceberQueryValidation : AbstractValidator<ObterContaReceberQuery>
{
    public ObterContaReceberQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
