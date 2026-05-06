using FluentValidation;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

public sealed class ObterFluxoQueryValidation : AbstractValidator<ObterFluxoQuery>
{
    public ObterFluxoQueryValidation()
    {
        RuleFor(x => x.Inicio).NotEmpty();
        RuleFor(x => x.Fim).NotEmpty()
            .GreaterThanOrEqualTo(x => x.Inicio)
            .WithMessage("Fim deve ser maior ou igual a Inicio.");
    }
}
