using FluentValidation;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;

public sealed class EvolucaoFinanceiraQueryValidation : AbstractValidator<EvolucaoFinanceiraQuery>
{
    public EvolucaoFinanceiraQueryValidation()
    {
        RuleFor(x => x.Meses).InclusiveBetween(1, 36);
    }
}
