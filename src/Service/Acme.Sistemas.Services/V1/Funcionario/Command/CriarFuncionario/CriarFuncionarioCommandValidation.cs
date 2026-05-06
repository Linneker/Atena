using Acme.Sistemas.Core.Helper;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

public sealed class CriarFuncionarioCommandValidation : AbstractValidator<CriarFuncionarioCommand>
{
    public CriarFuncionarioCommandValidation()
    {
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Cpf).NotEmpty().Must(CpfHelper.IsValid).WithMessage("CPF inválido.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Cargo).MaximumLength(100);
        RuleFor(x => x.Departamento).MaximumLength(100);
    }
}
