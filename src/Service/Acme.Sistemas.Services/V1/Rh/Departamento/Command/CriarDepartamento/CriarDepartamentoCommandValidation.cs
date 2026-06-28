using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.CriarDepartamento;

public sealed class CriarDepartamentoCommandValidation : AbstractValidator<CriarDepartamentoCommand>
{
    public CriarDepartamentoCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Codigo).MaximumLength(20);
    }
}
