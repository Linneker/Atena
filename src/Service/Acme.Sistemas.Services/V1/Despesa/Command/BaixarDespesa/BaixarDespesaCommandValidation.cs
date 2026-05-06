using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

public sealed class BaixarDespesaCommandValidation : AbstractValidator<BaixarDespesaCommand>
{
    public BaixarDespesaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValorPago).GreaterThan(0).WithMessage("Valor pago deve ser maior que zero.");
        RuleFor(x => x.DataPagamento).NotEmpty()
            .Must(d => d.Date <= DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Data de pagamento não pode ser futura.");
        RuleFor(x => x.Observacao).MaximumLength(500);
    }
}
