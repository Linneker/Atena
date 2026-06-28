using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

public sealed class ObterEspelhoMensalQueryValidation : AbstractValidator<ObterEspelhoMensalQuery>
{
    public ObterEspelhoMensalQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$")
            .WithMessage("competência deve estar no formato YYYY-MM.");
    }
}
