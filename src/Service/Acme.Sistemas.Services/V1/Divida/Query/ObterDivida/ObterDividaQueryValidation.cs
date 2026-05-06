using FluentValidation;

namespace Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

public sealed class ObterDividaQueryValidation : AbstractValidator<ObterDividaQuery>
{
    public ObterDividaQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
