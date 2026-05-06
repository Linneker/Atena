using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;

public sealed class BaixarContaPagarCommandValidation : AbstractValidator<BaixarContaPagarCommand>
{
    public BaixarContaPagarCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValorPago).GreaterThan(0);
        RuleFor(x => x.DataPagamento).NotEmpty()
            .Must(d => d.Date <= DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Data de pagamento não pode ser futura.");
        RuleFor(x => x.Observacao).MaximumLength(500);
    }
}
