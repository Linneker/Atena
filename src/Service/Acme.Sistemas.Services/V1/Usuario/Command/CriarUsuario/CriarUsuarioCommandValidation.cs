using FluentValidation;

namespace Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

public sealed class CriarUsuarioCommandValidation : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioCommandValidation()
    {
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(8).MaximumLength(100);
    }
}
