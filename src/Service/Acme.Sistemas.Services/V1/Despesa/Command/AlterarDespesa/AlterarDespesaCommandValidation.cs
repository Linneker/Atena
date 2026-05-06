using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

public sealed class AlterarDespesaCommandValidation : AbstractValidator<AlterarDespesaCommand>
{
    public AlterarDespesaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.Categoria).MaximumLength(100);
        RuleFor(x => x.Valor).GreaterThan(0);
        RuleFor(x => x.DataVencimento).NotEmpty();
    }
}
