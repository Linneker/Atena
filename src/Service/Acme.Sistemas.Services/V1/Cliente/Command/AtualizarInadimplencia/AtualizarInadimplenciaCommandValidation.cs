using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;

public sealed class AtualizarInadimplenciaCommandValidation : AbstractValidator<AtualizarInadimplenciaCommand>
{
    public AtualizarInadimplenciaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
