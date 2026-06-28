using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.RejeitarAjuste;

public sealed class RejeitarAjusteCommandValidation : AbstractValidator<RejeitarAjusteCommand>
{
    public RejeitarAjusteCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Justificativa).NotEmpty().MaximumLength(2000)
            .WithMessage("Justificativa de rejeição é obrigatória.");
    }
}
