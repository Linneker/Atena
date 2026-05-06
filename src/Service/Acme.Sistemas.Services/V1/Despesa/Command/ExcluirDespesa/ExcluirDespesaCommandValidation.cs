using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Command.ExcluirDespesa;

public sealed class ExcluirDespesaCommandValidation : AbstractValidator<ExcluirDespesaCommand>
{
    public ExcluirDespesaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
