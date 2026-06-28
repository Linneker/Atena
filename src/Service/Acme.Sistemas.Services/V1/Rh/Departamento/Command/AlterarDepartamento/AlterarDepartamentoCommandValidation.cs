using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.AlterarDepartamento;

public sealed class AlterarDepartamentoCommandValidation : AbstractValidator<AlterarDepartamentoCommand>
{
    public AlterarDepartamentoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Codigo).MaximumLength(20);
    }
}
