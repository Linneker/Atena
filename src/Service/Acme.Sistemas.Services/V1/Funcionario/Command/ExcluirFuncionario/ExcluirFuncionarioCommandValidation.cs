using FluentValidation;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;

public sealed class ExcluirFuncionarioCommandValidation : AbstractValidator<ExcluirFuncionarioCommand>
{
    public ExcluirFuncionarioCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
