using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

public sealed class ListarStatusFechamentoQueryValidation : AbstractValidator<ListarStatusFechamentoQuery>
{
    public ListarStatusFechamentoQueryValidation()
    {
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
    }
}
