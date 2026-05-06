using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

public sealed class ExcluirReceitaCommandValidation : AbstractValidator<ExcluirReceitaCommand>
{
    public ExcluirReceitaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
