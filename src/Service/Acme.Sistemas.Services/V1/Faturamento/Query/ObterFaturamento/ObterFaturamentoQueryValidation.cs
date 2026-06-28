using FluentValidation;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

public sealed class ObterFaturamentoQueryValidation : AbstractValidator<ObterFaturamentoQuery>
{
    public ObterFaturamentoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
