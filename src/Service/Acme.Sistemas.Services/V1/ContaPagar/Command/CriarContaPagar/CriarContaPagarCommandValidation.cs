using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;

public sealed class CriarContaPagarCommandValidation : AbstractValidator<CriarContaPagarCommand>
{
    public CriarContaPagarCommandValidation()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ValorOriginal).GreaterThan(0);
        RuleFor(x => x.DataVencimento).NotEmpty();
        RuleFor(x => x.Observacao).MaximumLength(2000);
    }
}
