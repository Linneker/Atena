using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

public sealed class BaterPontoCommandValidation : AbstractValidator<BaterPontoCommand>
{
    public BaterPontoCommandValidation()
    {
        RuleFor(x => x.Latitude).InclusiveBetween(-90m, 90m).When(x => x.Latitude.HasValue);
        RuleFor(x => x.Longitude).InclusiveBetween(-180m, 180m).When(x => x.Longitude.HasValue);
        RuleFor(x => x.IpOrigem).MaximumLength(45);
        RuleFor(x => x.UserAgent).MaximumLength(255);
    }
}
