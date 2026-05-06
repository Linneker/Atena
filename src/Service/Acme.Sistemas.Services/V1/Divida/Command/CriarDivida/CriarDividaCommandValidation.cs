using FluentValidation;

namespace Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

public sealed class CriarDividaCommandValidation : AbstractValidator<CriarDividaCommand>
{
    public CriarDividaCommandValidation()
    {
        RuleFor(x => x.Credor).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.ValorOriginal).GreaterThan(0);
        RuleFor(x => x.TaxaJurosMensal).GreaterThanOrEqualTo(0).When(x => x.TaxaJurosMensal.HasValue);
        RuleFor(x => x.NumeroParcelas).GreaterThan(0);
        RuleFor(x => x.DataInicio).NotEmpty();
    }
}
