using FluentValidation;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

public sealed class ConfirmarEmailCommandValidation : AbstractValidator<ConfirmarEmailCommand>
{
    public ConfirmarEmailCommandValidation()
    {
        RuleFor(x => x.Token).NotEmpty().MaximumLength(256);
    }
}
