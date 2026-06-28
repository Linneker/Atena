using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.VincularBeneficio;

public sealed class VincularBeneficioCommandValidation : AbstractValidator<VincularBeneficioCommand>
{
    public VincularBeneficioCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.BeneficioCatalogoId).NotEmpty();
        RuleFor(x => x.Valor).GreaterThanOrEqualTo(0).When(x => x.Valor.HasValue);
        RuleFor(x => x.DescontoFuncionarioPct).InclusiveBetween(0, 100)
            .When(x => x.DescontoFuncionarioPct.HasValue);
    }
}
