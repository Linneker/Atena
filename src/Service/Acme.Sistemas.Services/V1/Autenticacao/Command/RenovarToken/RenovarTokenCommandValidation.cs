using FluentValidation;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.RenovarToken;

public sealed class RenovarTokenCommandValidation : AbstractValidator<RenovarTokenCommand>
{
    public RenovarTokenCommandValidation()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
