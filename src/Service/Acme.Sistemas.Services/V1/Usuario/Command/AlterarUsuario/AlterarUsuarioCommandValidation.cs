using FluentValidation;

namespace Acme.Sistemas.Services.V1.Usuario.Command.AlterarUsuario;

public sealed class AlterarUsuarioCommandValidation : AbstractValidator<AlterarUsuarioCommand>
{
    public AlterarUsuarioCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Status).IsInEnum();
    }
}
