using FluentValidation;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;

public sealed class AlterarFuncionarioCommandValidation : AbstractValidator<AlterarFuncionarioCommand>
{
    public AlterarFuncionarioCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
