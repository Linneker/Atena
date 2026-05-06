using FluentValidation;

namespace Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;

public sealed class AlterarDividaCommandValidation : AbstractValidator<AlterarDividaCommand>
{
    public AlterarDividaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Credor).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.ValorOriginal).GreaterThan(0);
        RuleFor(x => x.NumeroParcelas).GreaterThan(0);
    }
}
