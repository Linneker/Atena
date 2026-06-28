using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

public sealed class ListarLotacoesQueryValidation : AbstractValidator<ListarLotacoesQuery>
{
    public ListarLotacoesQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
