using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

public sealed class AprovarAjusteCommandValidation : AbstractValidator<AprovarAjusteCommand>
{
    public AprovarAjusteCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Justificativa).MaximumLength(2000);
    }
}
