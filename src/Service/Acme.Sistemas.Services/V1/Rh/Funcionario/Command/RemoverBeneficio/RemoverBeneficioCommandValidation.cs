using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverBeneficio;

public sealed class RemoverBeneficioCommandValidation : AbstractValidator<RemoverBeneficioCommand>
{
    public RemoverBeneficioCommandValidation()
    {
        RuleFor(x => x.VinculoId).NotEmpty();
    }
}
