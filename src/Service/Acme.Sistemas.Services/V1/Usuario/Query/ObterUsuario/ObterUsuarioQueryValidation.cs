using FluentValidation;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

public sealed class ObterUsuarioQueryValidation : AbstractValidator<ObterUsuarioQuery>
{
    public ObterUsuarioQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
