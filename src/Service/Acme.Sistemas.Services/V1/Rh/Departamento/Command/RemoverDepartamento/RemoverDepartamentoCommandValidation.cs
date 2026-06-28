using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.RemoverDepartamento;

public sealed class RemoverDepartamentoCommandValidation : AbstractValidator<RemoverDepartamentoCommand>
{
    public RemoverDepartamentoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
