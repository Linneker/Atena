using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

public sealed class ObterFichaCompletaQueryValidation : AbstractValidator<ObterFichaCompletaQuery>
{
    public ObterFichaCompletaQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
    }
}
