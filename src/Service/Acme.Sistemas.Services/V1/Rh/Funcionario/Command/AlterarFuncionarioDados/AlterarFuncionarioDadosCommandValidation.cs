using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioDados;

public sealed class AlterarFuncionarioDadosCommandValidation : AbstractValidator<AlterarFuncionarioDadosCommand>
{
    public AlterarFuncionarioDadosCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
