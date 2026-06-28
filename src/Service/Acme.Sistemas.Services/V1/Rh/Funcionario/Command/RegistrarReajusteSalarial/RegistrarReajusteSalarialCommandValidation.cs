using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

public sealed class RegistrarReajusteSalarialCommandValidation : AbstractValidator<RegistrarReajusteSalarialCommand>
{
    public RegistrarReajusteSalarialCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.NovoValor).GreaterThan(0);
    }
}
