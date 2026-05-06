using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

public sealed class AlterarReceitaCommandValidation : AbstractValidator<AlterarReceitaCommand>
{
    public AlterarReceitaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.Categoria).MaximumLength(100);
        RuleFor(x => x.Valor).GreaterThan(0);
        RuleFor(x => x.DataPrevistaRecebimento).NotEmpty();
    }
}
