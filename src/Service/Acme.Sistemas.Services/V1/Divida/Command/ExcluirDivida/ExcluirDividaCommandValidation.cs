using FluentValidation;

namespace Acme.Sistemas.Services.V1.Divida.Command.ExcluirDivida;

public sealed class ExcluirDividaCommandValidation : AbstractValidator<ExcluirDividaCommand>
{
    public ExcluirDividaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
