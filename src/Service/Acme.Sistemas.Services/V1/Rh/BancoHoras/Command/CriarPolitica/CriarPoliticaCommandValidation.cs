using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CriarPolitica;

public sealed class CriarPoliticaCommandValidation : AbstractValidator<CriarPoliticaCommand>
{
    public CriarPoliticaCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.LimiteHorasAcumular).GreaterThan(0).LessThanOrEqualTo(1000);
        RuleFor(x => x.PrazoCompensacaoDias).InclusiveBetween(1, 730);
        RuleFor(x => x.FatorPagamento).InclusiveBetween(1m, 3m);
    }
}
