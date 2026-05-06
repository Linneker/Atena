using FluentValidation;

namespace Acme.Sistemas.Services.V1.Relatorios.Vendas;

public sealed class RelatorioVendasQueryValidation : AbstractValidator<RelatorioVendasQuery>
{
    public RelatorioVendasQueryValidation()
    {
        RuleFor(x => x.Inicio).NotEmpty();
        RuleFor(x => x.Fim).NotEmpty().GreaterThanOrEqualTo(x => x.Inicio);
        RuleFor(x => x.Agrupamento).IsInEnum();
    }
}
