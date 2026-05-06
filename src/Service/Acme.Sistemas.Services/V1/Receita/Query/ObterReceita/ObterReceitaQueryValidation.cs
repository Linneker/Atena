using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

public sealed class ObterReceitaQueryValidation : AbstractValidator<ObterReceitaQuery>
{
    public ObterReceitaQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
