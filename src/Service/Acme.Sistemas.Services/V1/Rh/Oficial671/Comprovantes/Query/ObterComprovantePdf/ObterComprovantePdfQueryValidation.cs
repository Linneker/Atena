using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

public sealed class ObterComprovantePdfQueryValidation : AbstractValidator<ObterComprovantePdfQuery>
{
    public ObterComprovantePdfQueryValidation() => RuleFor(x => x.MarcacaoId).NotEmpty();
}
