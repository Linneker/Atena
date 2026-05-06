using FluentValidation;

namespace Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

public sealed class ExcluirUsuarioCommandValidation : AbstractValidator<ExcluirUsuarioCommand>
{
    public ExcluirUsuarioCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
