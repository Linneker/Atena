using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

public sealed class ReceberReceitaCommandValidation : AbstractValidator<ReceberReceitaCommand>
{
    public ReceberReceitaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValorRecebido).GreaterThan(0).WithMessage("Valor recebido deve ser maior que zero.");
        RuleFor(x => x.DataRecebimento).NotEmpty()
            .Must(d => d.Date <= DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Data de recebimento não pode ser futura.");
        RuleFor(x => x.Observacao).MaximumLength(500);
    }
}
