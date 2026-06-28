using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

public sealed class BaterPontoMobileCommandValidation : AbstractValidator<BaterPontoMobileCommand>
{
    public BaterPontoMobileCommandValidation()
    {
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(120);
        RuleFor(x => x.HashBatida).NotEmpty().Length(64);
        RuleFor(x => x.Latitude).InclusiveBetween(-90m, 90m).When(x => x.Latitude.HasValue);
        RuleFor(x => x.Longitude).InclusiveBetween(-180m, 180m).When(x => x.Longitude.HasValue);
        RuleFor(x => x.FotoBytes).Must(b => b is null || b.Length <= 5 * 1024 * 1024)
            .WithMessage("foto deve ter no máximo 5MB.");
    }
}
