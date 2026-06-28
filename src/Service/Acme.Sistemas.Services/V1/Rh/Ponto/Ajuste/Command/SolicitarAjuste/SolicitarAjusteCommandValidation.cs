using Acme.Sistemas.Domain.Enums;
using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.SolicitarAjuste;

public sealed class SolicitarAjusteCommandValidation : AbstractValidator<SolicitarAjusteCommand>
{
    public SolicitarAjusteCommandValidation()
    {
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.MarcacaoOriginalId).NotEmpty()
            .When(x => x.TipoAjuste != TipoAjuste.Inclusao);
        RuleFor(x => x.DataHoraProposta).NotEmpty()
            .When(x => x.TipoAjuste == TipoAjuste.Inclusao || x.TipoAjuste == TipoAjuste.AlteracaoHora);
        RuleFor(x => x.TipoMarcacaoProposta).NotNull()
            .When(x => x.TipoAjuste == TipoAjuste.Inclusao);
    }
}
