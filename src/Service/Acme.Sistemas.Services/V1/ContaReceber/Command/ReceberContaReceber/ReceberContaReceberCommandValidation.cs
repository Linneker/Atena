using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

public sealed class ReceberContaReceberCommandValidation : AbstractValidator<ReceberContaReceberCommand>
{
    public ReceberContaReceberCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValorRecebido).GreaterThan(0);
        RuleFor(x => x.DataRecebimento).NotEmpty()
            .Must(d => d.Date <= DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Data de recebimento não pode ser futura.");
        RuleFor(x => x.Observacao).MaximumLength(2000);
    }
}
