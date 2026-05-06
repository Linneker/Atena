using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

public sealed class CriarDespesaCommandValidation : AbstractValidator<CriarDespesaCommand>
{
    public CriarDespesaCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.Categoria).MaximumLength(100);
        RuleFor(x => x.Valor).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(x => x.DataVencimento).NotEmpty();
    }
}
