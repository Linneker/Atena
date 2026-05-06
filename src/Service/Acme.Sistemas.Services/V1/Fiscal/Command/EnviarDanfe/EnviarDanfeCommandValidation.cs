using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EnviarDanfe;

public sealed class EnviarDanfeCommandValidation : AbstractValidator<EnviarDanfeCommand>
{
    public EnviarDanfeCommandValidation()
    {
        RuleFor(x => x.NFeId).NotEmpty();
        RuleFor(x => x.EmailDestinoOverride).EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.EmailDestinoOverride));
    }
}
