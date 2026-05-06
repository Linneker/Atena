using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;

public sealed class EnviarParaAprovacaoCommandValidation : AbstractValidator<EnviarParaAprovacaoCommand>
{
    public EnviarParaAprovacaoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
