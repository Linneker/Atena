using FluentValidation;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

public sealed class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {
        RuleFor(x => x.Cnpj).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(1).MaximumLength(200);
    }
}
