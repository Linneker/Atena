using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

public sealed class ObterLotacaoQueryValidation : AbstractValidator<ObterLotacaoQuery>
{
    public ObterLotacaoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
