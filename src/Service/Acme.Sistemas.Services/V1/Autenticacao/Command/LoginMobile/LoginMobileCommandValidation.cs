using FluentValidation;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

public sealed class LoginMobileCommandValidation : AbstractValidator<LoginMobileCommand>
{
    public LoginMobileCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(8);
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Plataforma).NotEmpty()
            .Must(p => new[] { "Android", "iOS", "MacCatalyst", "Mac", "WinUI", "Windows" }
                       .Contains(p, StringComparer.OrdinalIgnoreCase))
            .WithMessage("plataforma deve ser Android, iOS, MacCatalyst ou Windows.");
    }
}
