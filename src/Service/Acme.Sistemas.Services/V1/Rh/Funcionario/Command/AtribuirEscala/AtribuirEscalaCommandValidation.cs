using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

public sealed class AtribuirEscalaCommandValidation : AbstractValidator<AtribuirEscalaCommand>
{
    public AtribuirEscalaCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.JornadaId).NotEmpty();
    }
}
