using FluentValidation;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

public sealed class LogoutCommandValidation : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidation()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
