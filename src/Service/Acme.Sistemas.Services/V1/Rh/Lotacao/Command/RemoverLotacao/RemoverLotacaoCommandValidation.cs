using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.RemoverLotacao;

public sealed class RemoverLotacaoCommandValidation : AbstractValidator<RemoverLotacaoCommand>
{
    public RemoverLotacaoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
