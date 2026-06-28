using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioContrato;

public sealed class AlterarFuncionarioContratoCommandValidation : AbstractValidator<AlterarFuncionarioContratoCommand>
{
    public AlterarFuncionarioContratoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CodigoMatricula).MaximumLength(30);
    }
}
